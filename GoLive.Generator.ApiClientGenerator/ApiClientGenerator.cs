﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GoLive.Generator.ApiClientGenerator
{
    [Generator]
    public class ApiClientGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context) {
            IncrementalValuesProvider<ControllerRoute> controllerDeclarations = context.SyntaxProvider
               .CreateSyntaxProvider(
                    predicate: static (s, _) => Scanner.CanBeController(s), 
                    transform: static (ctx, _) => GetControllerDeclarations(ctx))
               .Where(static c => c is not null)
               .Select(static (c, _) => Scanner.ConvertToRoute(c));

            var configFiles = context.AdditionalTextsProvider.Where(IsConfigurationFile);
            var controllersAndConfig = controllerDeclarations.Collect().Combine(configFiles.Collect());
            context.RegisterSourceOutput(controllersAndConfig, 
                static (spc, source) => Execute(source.Left, source.Right, spc));
        }

        private static INamedTypeSymbol GetControllerDeclarations(GeneratorSyntaxContext context)
        {
            // we know the node is a ClassDeclarationSyntax thanks to CanBeController
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

            var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            return symbol is not null && Scanner.IsController(symbol) ? symbol : null;
        }

        private static bool IsConfigurationFile(AdditionalText text)
            => text.Path.EndsWith("ApiClientGenerator.json");
        
        public static void Execute(
            ImmutableArray<ControllerRoute> controllerRoutes,
            IEnumerable<AdditionalText> configurationFiles, SourceProductionContext context) 
        {
            var config = LoadConfig(configurationFiles);

            var source = new SourceStringBuilder();

            if (config.PreAppendLines is { Count: > 0 })
            {
                config.PreAppendLines.ForEach(source.AppendLine);
            }
            
            source.AppendLine("using System.Net.Http;");
            source.AppendLine("using System.Threading.Tasks;");
            source.AppendLine("using System.Net.Http.Json;");
            source.AppendLine("using System.Collections.Generic;");
            source.AppendLine("using System.Threading;");
            source.AppendLine("using System.Diagnostics.CodeAnalysis;");

            if (config.UseResponseWrapper)
            {
                source.AppendLine("using System.Net;");
            }

            if (config.Includes != null && config.Includes.Any())
            {
                foreach (var s in config.Includes)
                {
                    source.AppendLine($"using {s};");
                }
            }

            source.AppendLine();

            source.AppendLine(!string.IsNullOrWhiteSpace(config.Namespace)
                ? $"namespace {config.Namespace}"
                : "namespace AutogeneratedClient");


            source.AppendOpenCurlyBracketLine();

            var orderedControllerRoutes = controllerRoutes
                                         .OrderBy(r => r.Name)
                                         .ToArray();

            SetUpApiClient(config, orderedControllerRoutes, source);

            foreach (var route in orderedControllerRoutes)
            {
                SetUpSingleApi(config, route, source);
            }

            source.AppendCloseCurlyBracketLine();

            if (config.PostAppendLines is { Count: > 0 })
            {
                config.PostAppendLines.ForEach(source.AppendLine);
            }

            if ((string.IsNullOrWhiteSpace(config.OutputFile) && (config.OutputFiles == null || config.OutputFiles.Count == 0)))
            {
                context.AddSource("GeneratedApiClient", source.ToString());
            }
            else
            {
                SaveSourceToFile(config, source);
            }
        }

        private static void SaveSourceToFile(RouteGeneratorSettings config, SourceStringBuilder source)
        {
            var content = source.ToString();
            if (config.OutputFiles is { Count: > 0 })
            {
                foreach (var configOutputFile in config.OutputFiles)
                {
                    File.WriteAllText(configOutputFile, content);
                }
            }
            else
            {
                File.WriteAllText(config.OutputFile, content);
            }
        }

        private static RouteGeneratorSettings LoadConfig(IEnumerable<AdditionalText> configFiles)
        {
            var configFilePath = configFiles.FirstOrDefault();

            if (configFilePath == null)
            {
                return null;
            }

            var jsonString = File.ReadAllText(configFilePath.Path);
            var config = JsonSerializer.Deserialize<RouteGeneratorSettings>(jsonString);
            var configFileDirectory = Path.GetDirectoryName(configFilePath.Path);

            var fullPath = Path.Combine(configFileDirectory, config.OutputFile);
            config.OutputFile = Path.GetFullPath(fullPath);

            if (config.OutputFiles is { Count: > 0 })
            {
                config.OutputFiles = config.OutputFiles.Select(e => Path.GetFullPath(Path.Combine(configFileDirectory, e))).ToList();
            }

            return config;
        }
        
        private static void SetUpSingleApi(RouteGeneratorSettings config, ControllerRoute controllerRoute, SourceStringBuilder source)
        {
            source.AppendLine();
            string className;

            if (controllerRoute.Area != null && !string.IsNullOrWhiteSpace(controllerRoute.Area))
            {
                className = $"{controllerRoute.Area}_{controllerRoute.Name}";
            }
            else
            {
                className = controllerRoute.Name;
            }

            className = $"{className}Client";

            source.AppendLine($"public class {className}");
            source.AppendOpenCurlyBracketLine();

            source.AppendLine("private readonly HttpClient _client;");

            source.AppendLine();
            source.AppendLine($"public {className} (HttpClient client)");

            source.AppendOpenCurlyBracketLine();
            source.AppendLine("_client = client;");
            source.AppendCloseCurlyBracketLine();

            foreach (var action in controllerRoute.Actions)
            {
                bool byteReturnType = action.ReturnTypeName == "byte[]";

                var parameterList = string.Join(", ", action.Mapping.Select(m => $"{m.Parameter.FullTypeName} {m.Key} {GetDefaultValue(m.Parameter)}"));

                bool containsFileUpload = action.Mapping.Any(f => f.Parameter.FullTypeName == "Microsoft.AspNetCore.Http.IFormFile");

                if (containsFileUpload) {
                    parameterList = string.Join(", ",
                        action.Mapping.Select(m =>
                            m.Parameter.FullTypeName == "Microsoft.AspNetCore.Http.IFormFile"
                                ? "System.Net.Http.MultipartFormDataContent multiPartContent"
                                : $"{m.Parameter.FullTypeName} {m.Key} {GetDefaultValue(m.Parameter)}"));
                }

                string useCustomFormatter = config.CustomDiscriminator;
                
                string routeValue = string.Empty;

                if (!string.IsNullOrWhiteSpace(action.Route))
                {
                    // TODO need to replace params
                    routeValue = action.Route;
                }
                else
                {
                    if (controllerRoute.Area != null && !string.IsNullOrWhiteSpace(controllerRoute.Area))
                    {
                        routeValue = $"/{controllerRoute.Area}/{controllerRoute.Name}/{action.Name}";
                    }
                    else
                    {
                        routeValue = $"/{controllerRoute.Name}/{action.Name}";
                    }

                    if (!routeValue.ToLower().EndsWith("{id}"))
                    {
                        if (action.Mapping.FirstOrDefault(e => e.Key.ToLower() == "id") != null)
                        {
                            routeValue = $"{routeValue}/{{{action.Mapping.FirstOrDefault(e => e.Key.ToLower() == "id")?.Key}}}";
                        }
                    }
                }

                routeValue = routeValue.TrimStart('~');
                routeValue = routeValue.Replace("*", ""); // TODO - to remove greedy url params

                if (!string.IsNullOrWhiteSpace(config.PrefixUrl) && !action.RouteSetByAttributes) // TODO want to change this to use a single way to determine routes, not prefixing
                {
                    routeValue = $"{config.PrefixUrl}{routeValue}";
                }
                
                var routeString = $"$\"{routeValue}\"";

                if (config.HideUrlsRegex is { Count: > 0 })
                {
                    if (config.HideUrlsRegex.Any(e => Regex.IsMatch(routeValue, e)))
                    {
                        continue;
                    }
                }
                
                
                routeString = routeString.Replace("[controller]", controllerRoute.Name);
                routeString = routeString.Replace("[action]", action.Name);
                
                List<string> routeParameters = new();
                
                if (routeString.Contains("{"))
                {
                    var regexPattern = @".*?\{(?<param>.*?)}.*?";
                    var matches = Regex.Matches(routeString,regexPattern,RegexOptions.Multiline);
                    routeParameters.AddRange(matches.Cast<Match>().Select(match => match.Groups["param"].Value.Contains(":") ? match.Groups["param"].Value.Substring(0, match.Groups["param"].Value.IndexOf(":", StringComparison.Ordinal)) : match.Groups["param"].Value));

                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            var orig = match.Groups["param"].Value;

                            if (orig.Contains(":"))
                            {
                                routeString = routeString.Replace(orig, orig.Substring(0, orig.IndexOf(":")));
                            }
                        }
                    }
                }
                
                source.AppendLine();

                var nullableReturnType = action.ReturnTypeStruct || action.ReturnTypeName.EndsWith("?")
                    ? action.ReturnTypeName
                    : $"{action.ReturnTypeName}?";

                var returnType = config.UseResponseWrapper switch {
                    true when action.ReturnTypeName is null => "Task<Response>",
                    true => $"Task<Response<{action.ReturnTypeName}>>",
                    false when action.ReturnTypeName is null => "Task",
                    false => $"Task<{nullableReturnType}>"
                };

                source.AppendLine(string.IsNullOrWhiteSpace(parameterList)
                    ? $"public async {returnType} {action.Name}(CancellationToken _token = default)"
                    : $"public async {returnType} {action.Name}({parameterList}, CancellationToken _token = default)");

                source.AppendOpenCurlyBracketLine();

                if (action.Mapping.Any(f => f.Key.ToLower() != "id" && action.Body?.Key != f.Key && !routeParameters.Contains(f.Key)  ))
                {
                    source.AppendLine("Dictionary<string, string> queryString=new();");

                    foreach (var parameterMapping in action.Mapping.Where(f => f.Key != "Id" && action.Body?.Key != f.Key && !routeParameters.Contains(f.Key)))
                    {
                        if (parameterMapping.Parameter.FullTypeName == "string")
                        {
                            source.AppendLine($"if (!string.IsNullOrWhiteSpace({parameterMapping.Key}))");
                        }
                        else
                        {
                            source.AppendLine($"if ({parameterMapping.Key} != default)");
                        }

                        source.AppendOpenCurlyBracketLine();
                        source.AppendLine($"queryString.Add(\"{parameterMapping.Key}\", {parameterMapping.Key}.ToString());");
                        source.AppendCloseCurlyBracketLine();
                    }

                    routeString = $"Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString({routeString}, queryString)";
                }


                var methodString = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(action.Method.Method.ToLower());

                //if (methodString == "Post" && action.Body == null)
                //{
                //    action.Body = new ParameterMapping()
                //}

                string callStatement;

                if (containsFileUpload)
                {
                    callStatement = $"await _client.{methodString}Async({routeString}, multiPartContent, cancellationToken: _token);";
                }
                else if (action.Body is { Key: var key })
                {
                    if (string.IsNullOrWhiteSpace(useCustomFormatter))
                    {
                        callStatement = $"await _client.{methodString}AsJsonAsync({routeString}, {key}, cancellationToken: _token);";
                    }
                    else
                    {
                        callStatement = $"await _client.{methodString}AsJsonAsync({routeString}, {key}, {useCustomFormatter}, cancellationToken: _token);";
                    }
                }
                else if (methodString == "Post")
                {
                    callStatement = $"await _client.{methodString}AsJsonAsync({routeString}, new {{}}, cancellationToken: _token);";
                }
                else
                {
                    callStatement = $"await _client.{methodString}Async({routeString}, cancellationToken: _token);";
                }

                if (action.ReturnTypeName == null)
                {
                    if (config.UseResponseWrapper)
                    {
                        source.AppendLine($"using var result = {callStatement}");
                        source.AppendLine($"return new Response(result.StatusCode);");
                    }
                    else
                    {
                        source.AppendLine(callStatement);
                    }
                }
                else
                {
                    source.AppendLine($"using var result = {callStatement}");

                    string readValue;
                    if (byteReturnType)
                    {
                        readValue = "result.Content?.ReadAsByteArrayAsync()";
                    }
                    else if (string.IsNullOrWhiteSpace(useCustomFormatter))
                    {
                        readValue = $"result.Content?.ReadFromJsonAsync<{action.ReturnTypeName}>(cancellationToken: _token)";
                    }
                    else
                    {
                        readValue = $"result.Content?.ReadFromJsonAsync<{action.ReturnTypeName}>({useCustomFormatter}, cancellationToken: _token)";
                    }
                    
                    source.AppendMultipleLines(config.UseResponseWrapper
                        ? $"""
                        return new Response<{action.ReturnTypeName}>(
                            result.StatusCode,
                            await ({readValue} 
                                    ?? Task.FromResult<{nullableReturnType}>(default)));
                        """
                        : $"return await {readValue};");
                }

                source.AppendCloseCurlyBracketLine();

                if (config.OutputUrls)
                {
                    List<string> secondParamList = new();
                    if (action.Mapping.Any(f => action.Body?.Key != f.Key))
                    {
                        secondParamList.AddRange(action.Mapping.Where(f => action.Body?.Key != f.Key).Select(parameterMapping => $"{parameterMapping.Parameter.FullTypeName} {parameterMapping.Key} {GetDefaultValue(parameterMapping.Parameter)}"));
                    }


                    source.AppendLine($" public string {config.OutputUrlsPrefix}{action.Name}{config.OutputUrlsPostfix} ({string.Join(",", secondParamList)})");
                    
                    source.AppendOpenCurlyBracketLine();
                    if (action.Mapping.Any(f => f.Key.ToLower() != "id" && action.Body?.Key != f.Key && !routeParameters.Contains(f.Key)))
                    {
                        source.AppendLine("Dictionary<string, string> queryString=new();");
                        foreach (var parameterMapping in action.Mapping.Where(f => f.Key != "Id" && action.Body?.Key != f.Key && !routeParameters.Contains(f.Key)))
                        {
                            source.AppendLine(parameterMapping.Parameter.FullTypeName == "string" ? $"if (!string.IsNullOrWhiteSpace({parameterMapping.Key}))" : $"if ({parameterMapping.Key} != default)");
                            source.AppendOpenCurlyBracketLine();
                            source.AppendLine($"queryString.Add(\"{parameterMapping.Key}\", {parameterMapping.Key}.ToString());");
                            source.AppendCloseCurlyBracketLine();
                        }

                        source.AppendLine($"return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString({routeString}, queryString);");
                    }
                    else
                    {
                        source.AppendLine($"return {routeString};");
                    }
                    
                    source.AppendCloseCurlyBracketLine();
                }
            }

            source.AppendCloseCurlyBracketLine();
        }

        private static string GetDefaultValue(Parameter argParameter)
        {
            if (argParameter.HasDefaultValue)
            {
                return argParameter.DefaultValue switch
                {
                    null => " = null",
                    bool b => $" = {b.ToString().ToLower()}",
                    string e => " = \"\"",
                    _ => $" = {argParameter.DefaultValue}"
                };
            }

            return string.Empty;
        }

        private static void SetUpApiClient(RouteGeneratorSettings config, IEnumerable<ControllerRoute> routes, SourceStringBuilder source)
        {
            source.AppendLine();
            source.AppendLine("public class ApiClient");
            source.AppendOpenCurlyBracketLine();

            source.AppendLine("public ApiClient(HttpClient client)");
            source.AppendOpenCurlyBracketLine();

            foreach (var route in routes)
            {
                source.AppendLine($"{(string.IsNullOrWhiteSpace(route.Area) ? route.Name : $"{route.Area}_{route.Name}")} = new {(string.IsNullOrWhiteSpace(route.Area) ? route.Name : $"{route.Area}_{route.Name}")}Client(client);");
            }

            source.AppendCloseCurlyBracketLine();

            foreach (var route in routes)
            {
                source.AppendLine();
                source.AppendLine($"public {(string.IsNullOrWhiteSpace(route.Area) ? route.Name : $"{route.Area}_{route.Name}")}Client {(string.IsNullOrWhiteSpace(route.Area) ? route.Name : $"{route.Area}_{route.Name}")} {{ get; }}");
            }

            source.AppendCloseCurlyBracketLine();
            
            if (config.UseResponseWrapper)
            {
                source.AppendLine("public class Response");
                source.AppendOpenCurlyBracketLine();
                source.AppendLine("public Response() {}");
                source.AppendLine("public Response(HttpStatusCode statusCode)");
                source.AppendOpenCurlyBracketLine();
                source.AppendLine("StatusCode = statusCode;");
                source.AppendCloseCurlyBracketLine();
                source.AppendLine("public HttpStatusCode StatusCode { get; }");
                source.AppendLine("public bool Success => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);");
                source.AppendCloseCurlyBracketLine();
                source.AppendLine("public class Response<T> : Response");
                source.AppendOpenCurlyBracketLine();
                source.AppendLine("public Response() {}");
                source.AppendLine("public Response(HttpStatusCode statusCode, T? data) : base(statusCode)");
                source.AppendOpenCurlyBracketLine();
                source.AppendLine("Data = data;");
                source.AppendCloseCurlyBracketLine();
                source.AppendMultipleLines("""
                    public T? Data { get; }

                    public T SuccessData => Success ? Data ?? throw new NullReferenceException("Response had an empty body!")
                                                    : throw new InvalidOperationException("Request was not successful!");
                    """);
                source.AppendLine("public bool TryGetSuccessData([NotNullWhen(true)] out T? data)");
                source.AppendOpenCurlyBracketLine();
                source.AppendLine("data = Data;");
                source.AppendLine("return Success && data is not null;");
                source.AppendCloseCurlyBracketLine();
                source.AppendCloseCurlyBracketLine();
            }
        }
    }
}