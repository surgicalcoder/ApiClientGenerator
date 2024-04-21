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

namespace GoLive.Generator.ApiClientGenerator;

[Generator]
public class ApiClientGenerator : IIncrementalGenerator
{
    private const string TASK_FQ = "global::System.Threading.Tasks.Task";
        
    public void Initialize(IncrementalGeneratorInitializationContext context) 
    {
        IncrementalValuesProvider<ControllerRoute> controllerDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => Scanner.CanBeController(s), 
                transform: static (ctx, _) => GetControllerDeclarations(ctx))
            .Where(static c => c != default)
            .Select(static (c, _) => Scanner.ConvertToRoute(c.SemanticModel, c.symbol));

        var configFiles = context.AdditionalTextsProvider.Where(IsConfigurationFile);
        
        var controllersAndConfig = controllerDeclarations.Collect().Combine(configFiles.Collect());
        context.RegisterSourceOutput(controllersAndConfig, static (spc, source) => Execute(source.Left, source.Right, spc));
    }

    private static (INamedTypeSymbol symbol, SemanticModel SemanticModel) GetControllerDeclarations(GeneratorSyntaxContext context)
    {
        // we know the node is a ClassDeclarationSyntax thanks to CanBeController
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        return symbol is not null && Scanner.IsController(symbol) ? (symbol, context.SemanticModel) : default;
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
        source.AppendLine("using System.Text.Json.Serialization.Metadata;");

        if (config.OutputJSONSourceGenerator)
        {
            source.AppendLine("using System.Text.Json.Serialization;");
        }

        if (config.ResponseWrapper.Enabled)
        {
            source.AppendLine("using System.Net;");
            source.AppendLine("using System.Net.Http.Headers;");
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

        if (config.OutputJSONSourceGenerator)
        {
            List<string> ignoreTypesList =
            [
                "global::System.Threading.Tasks.Task"
            ];
            
            source.AppendLine("// JSON Source Generator");
            
            source.Append("[JsonSourceGenerationOptions(");

            if (config.JSONSourceGeneratorSettings != null)
            {
                Dictionary<string, string> jsonGenOptions = new();
                
                if (!string.IsNullOrWhiteSpace(config.JSONSourceGeneratorSettings.PropertyNamingPolicy))
                {
                    jsonGenOptions.Add("PropertyNamingPolicy",config.JSONSourceGeneratorSettings.PropertyNamingPolicy );
                }

                if (config.JSONSourceGeneratorSettings.AllowTrailingCommas)
                {
                    jsonGenOptions.Add("AllowTrailingCommas", "true");
                }

                if (config.JSONSourceGeneratorSettings.Converters is { Length: > 0 })
                {
                    var valsToUse = config.JSONSourceGeneratorSettings.Converters.Select(r => $"typeof({r})");
                    jsonGenOptions.Add("Converters", $"new[]{{ {string.Join(",", valsToUse)} }}");
                }
                
                source.Append(string.Join(",", jsonGenOptions.Select(r=> $"{r.Key} = {r.Value}" ) ));
                
                if (config.JSONSourceGeneratorSettings.AdditionalOptions is { Length: > 0 })
                {
                    source.Append(", ");
                    source.Append(string.Join(", ", config.JSONSourceGeneratorSettings.AdditionalOptions));
                }
            }
            
            source.Append(")]\n");

            var returnTypes = orderedControllerRoutes.SelectMany(e => e.Actions.Select(f => f.ReturnTypeName)).Distinct()
                .Where(r=>!string.IsNullOrWhiteSpace(r) && r.StartsWith("global::") ).Except(ignoreTypesList);
            
            foreach (var returnType in returnTypes)
            {
                source.AppendLine($"[JsonSerializable(typeof({returnType}))]");
            }
            
            source.AppendLine("public partial class ApiJsonSerializerContext : JsonSerializerContext");
            source.AppendOpenCurlyBracketLine();
            source.AppendCloseCurlyBracketLine();
            
            source.AppendLine("// JSON Source Generator");
        }

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

            if (config.Properties is { IgnoreTypes.Count: > 0 } || config.Properties.IgnoreGenericTypes?.Count > 0 || config.Properties.IgnoreThatHasAttribute?.Count > 0 || config.Properties.TransformType?.Count > 0 )
            {
                action.Mapping.RemoveAll(mapping => config.Properties.IgnoreTypes.Contains(mapping.Parameter.FullTypeName, StringComparer.InvariantCultureIgnoreCase));
                action.Mapping.RemoveAll(mapping => config.Properties.IgnoreGenericTypes.Contains(mapping.Parameter.GenericTypeName, StringComparer.InvariantCultureIgnoreCase));
                action.Mapping.RemoveAll(mapping => config.Properties.IgnoreThatHasAttribute.Intersect(mapping.Parameter.Attributes).Any());
                
                action.Body.RemoveAll(mapping => config.Properties.IgnoreTypes.Contains(mapping.Parameter.FullTypeName, StringComparer.InvariantCultureIgnoreCase));
                action.Body.RemoveAll(mapping => config.Properties.IgnoreGenericTypes.Contains(mapping.Parameter.GenericTypeName, StringComparer.InvariantCultureIgnoreCase));

                if (config.Properties.IgnoreThatHasAttribute.Count > 0)
                {
                    action.Body.RemoveAll(mapping => mapping.Parameter.Attributes != null && config.Properties.IgnoreThatHasAttribute.Intersect(mapping.Parameter.Attributes).Any());
                }

                if (config.Properties.TransformType?.Count > 0)
                {
                    foreach (var (key, parameter) in action.Mapping)
                    {
                        if (config.Properties.TransformType.FirstOrDefault(tt =>
                                (string.Equals(tt.SourceType, parameter.FullTypeName, StringComparison.InvariantCultureIgnoreCase) || tt.SourceType == "*") &&
                                (string.IsNullOrEmpty(tt.ContainsAttribute) || (parameter.Attributes.Count > 0 && parameter.Attributes.Contains(tt.ContainsAttribute)))) is {} tt)

                        {
                            parameter.FullTypeName = tt.DestinationType;
                        }
                    }

                    foreach (var (key, parameter) in action.Body)
                    {
                        if (config.Properties.TransformType.FirstOrDefault(tt =>
                                (string.Equals(tt.SourceType, parameter.FullTypeName, StringComparison.InvariantCultureIgnoreCase) || tt.SourceType == "*") &&
                                (string.IsNullOrEmpty(tt.ContainsAttribute) || (parameter.Attributes?.Count > 0 && parameter.Attributes.Contains(tt.ContainsAttribute)))) is {} tt)
                        {
                            parameter.FullTypeName = tt.DestinationType;
                        }
                    }
                }
                
            }

            var parameterList = string.Join(", ", action.Mapping.Select(m => $"{m.Parameter.FullTypeName} {m.Key} {GetDefaultValue(m.Parameter)}"));

            bool containsFileUpload = action.Mapping.Any(f => f.Parameter.FullTypeName == "Microsoft.AspNetCore.Http.IFormFile");

            if (containsFileUpload) {
                parameterList = string.Join(", ",
                    action.Mapping.Select(m =>
                        m.Parameter.FullTypeName == "Microsoft.AspNetCore.Http.IFormFile"
                            ? $"System.Net.Http.MultipartFormDataContent{(m.Parameter.Nullable ? "?" : "")} multiPartContent"
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
                
                

            string returnType = config.ResponseWrapper.Enabled switch
            {
                true when action.ReturnTypeName is null or TASK_FQ => "Task<Response>",
                true => $"Task<Response<{action.ReturnTypeName}>>",
                false when action.ReturnTypeName is null or TASK_FQ => "Task",
                false => $"Task<{nullableReturnType}>"
            };

            string jsonTypeInfoMethodParameter = (action.ReturnTypeName == null || action.ReturnTypeName == TASK_FQ || byteReturnType) ? string.Empty : $", JsonTypeInfo<{action.ReturnTypeName}> _typeInfo = default";
            string jsonTypeInfoMethodAppend = (action.ReturnTypeName == null || action.ReturnTypeName == TASK_FQ || byteReturnType) ? string.Empty : $", jsonTypeInfo: _typeInfo";

            source.AppendLine(string.IsNullOrWhiteSpace(parameterList)
                ? $"public async {returnType} {action.Name}(Dictionary<string, string?> queryString = default, CancellationToken _token = default {jsonTypeInfoMethodParameter})"
                : $"public async {returnType} {action.Name}({parameterList}, Dictionary<string, string?> queryString = default, CancellationToken _token = default {jsonTypeInfoMethodParameter})");

            source.AppendOpenCurlyBracketLine();
            
            if (config.OutputJSONSourceGenerator && (!string.IsNullOrWhiteSpace(action.ReturnTypeName) && action.ReturnTypeName != TASK_FQ && !byteReturnType))
            {
                source.AppendLine("if (_typeInfo == default)");
                source.AppendOpenCurlyBracketLine();
                source.AppendLine($"_typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof({action.ReturnTypeName})) as JsonTypeInfo<{action.ReturnTypeName}>;");
                source.AppendCloseCurlyBracketLine();
            }

            source.AppendLine("queryString ??= new();");
                
            // TODO do something with actual route parameters here
            if (action.Mapping.Any(f => !string.Equals(f.Key, "id", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(action.Body?.FirstOrDefault()?.Key, f.Key, StringComparison.InvariantCultureIgnoreCase) && !routeParameters.Contains(f.Key, StringComparer.InvariantCultureIgnoreCase)))
            {
                foreach (var parameterMapping in action.Mapping.Where(f => f.Key != "Id" && action.Body?.FirstOrDefault()?.Key != f.Key && !routeParameters.Contains(f.Key)))
                {
                    if (parameterMapping.Parameter.FullTypeName is "string" or "System.String") // TODO
                    {
                        source.AppendLine($"if (!string.IsNullOrWhiteSpace({parameterMapping.Key}) && !queryString.ContainsKey(\"{parameterMapping.Key}\") )"); // TODO need to fix to allow multiple keys with same value as allowed in http querystring
                    }
                    else
                    {
                        source.AppendLine($"if ({parameterMapping.Key} != default && !queryString.ContainsKey(\"{parameterMapping.Key}\") )"); // TODO need to fix to allow multiple keys with same value as allowed in http querystring
                    }

                    source.AppendOpenCurlyBracketLine();
                    source.AppendLine($"queryString.Add(\"{parameterMapping.Key}\", {parameterMapping.Key}.ToString());");
                    source.AppendCloseCurlyBracketLine();
                }
            }
            routeString = $"Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString({routeString}, queryString)";

            var methodString = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(action.Method.Method.ToLower());

            string callStatement;

            if (containsFileUpload)
            {
                callStatement = $"await _client.{methodString}Async({routeString}, multiPartContent, cancellationToken: _token);";
            }
            else if (action.Body is { Count: > 0 } && action.Body.FirstOrDefault() is { Key: var key } && !string.Equals(key, "id", StringComparison.InvariantCultureIgnoreCase))
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

            if (callStatement.Contains("await _client.GetAsJsonAsync"))
            {
                callStatement = callStatement.Replace("await _client.GetAsJsonAsync", "await _client.GetFromJsonAsync"); // Issue with GET JSON method being named differently.
            }

            if (action.ReturnTypeName is null or TASK_FQ)
            {
                if (config.ResponseWrapper.Enabled)
                {
                    source.AppendLine($"using var result = {callStatement}");
                    source.AppendLine($"return new Response(result.StatusCode, result.Headers);");
                }
                else
                {
                    source.AppendLine(callStatement);
                }
            }
            else
            {
                source.AppendLine($"using var result = {callStatement}");

                string readValueWithoutJsonTypeInformation;
                string readValue;
                if (byteReturnType)
                {
                    readValue = "result.Content?.ReadAsByteArrayAsync()";
                    readValueWithoutJsonTypeInformation = "result.Content?.ReadAsByteArrayAsync()";
                }
                else if (string.IsNullOrWhiteSpace(useCustomFormatter))
                {
                    readValue = $"result.Content?.ReadFromJsonAsync<{action.ReturnTypeName}>(cancellationToken: _token {jsonTypeInfoMethodAppend})";
                    readValueWithoutJsonTypeInformation = $"result.Content?.ReadFromJsonAsync<{action.ReturnTypeName}>(cancellationToken: _token)";
                }
                else
                {
                    readValue = $"result.Content?.ReadFromJsonAsync<{action.ReturnTypeName}>({useCustomFormatter}, cancellationToken: _token {jsonTypeInfoMethodAppend})";
                    readValueWithoutJsonTypeInformation = $"result.Content?.ReadFromJsonAsync<{action.ReturnTypeName}>({useCustomFormatter}, cancellationToken: _token)";
                }
                    
                if (!string.IsNullOrWhiteSpace(jsonTypeInfoMethodAppend))
                {
                    if (config.ResponseWrapper.Enabled)
                    {

                        source.AppendMultipleLines($"""
                                                    if (_typeInfo != default)
                                                        return new Response<{action.ReturnTypeName}>(result.StatusCode, result.Headers, ({readValue} ?? Task.FromResult<{nullableReturnType}>(default)));
                                                    else
                                                        return new Response<{action.ReturnTypeName}>(result.StatusCode, result.Headers, ({readValueWithoutJsonTypeInformation} ?? Task.FromResult<{nullableReturnType}>(default)));
                                                    """);
                    }
                    else
                    {
                        source.AppendLine("if (_typeInfo != default)");
                        using (source.CreateBracket())
                        {
                            source.AppendLine($"return await {readValue};");
                        }
                        source.AppendLine("else");
                        using (source.CreateBracket())
                        {
                            source.AppendLine($"return await {readValueWithoutJsonTypeInformation};");
                        }
                    }
                }
                else
                {
                    if (config.ResponseWrapper.Enabled)
                    {
                        source.AppendMultipleLines($"""
                                                    return new Response<{action.ReturnTypeName}>(
                                                        result.StatusCode,
                                                        result.Headers,
                                                        ({readValue}
                                                                ?? Task.FromResult<{nullableReturnType}>(default)));
                                                    """);
                    }
                    else
                    {
                        source.AppendLine($"return await {readValue};");
                    }
                }
                    
            }

            source.AppendCloseCurlyBracketLine();

            if (config.OutputUrls)
            {
                List<string> secondParamList = new();
                if (action.Mapping.Any(f => string.Equals(f.Key, "id", StringComparison.InvariantCultureIgnoreCase) || !string.Equals(action.Body?.FirstOrDefault()?.Key, f.Key, StringComparison.InvariantCultureIgnoreCase)))
                {
                    secondParamList.AddRange(action.Mapping.Where(f => string.Equals(f.Key, "id", StringComparison.InvariantCultureIgnoreCase) ||  action.Body?.FirstOrDefault()?.Key != f.Key).Select(parameterMapping => $"{parameterMapping.Parameter.FullTypeName} {parameterMapping.Key} {GetDefaultValue(parameterMapping.Parameter)}"));
                }
// TODO need to do something with all route values not just ID
                if (secondParamList.Any())
                {
                    source.AppendLine($"public string {config.OutputUrlsPrefix}{action.Name}{config.OutputUrlsPostfix} ({string.Join(",", secondParamList)}, Dictionary<string, string?> queryString = default)");
                }
                else
                {
                    source.AppendLine($"public string {config.OutputUrlsPrefix}{action.Name}{config.OutputUrlsPostfix} (Dictionary<string, string?> queryString = default)");
                }
                source.AppendOpenCurlyBracketLine();
                    
                source.AppendLine("queryString ??= new();");
                    
                if (action.Mapping.Any(f => f.Key.ToLower() != "id" && action.Body?.FirstOrDefault()?.Key != f.Key && !routeParameters.Contains(f.Key)))
                {
                    foreach (var parameterMapping in action.Mapping.Where(f => f.Key != "Id" && action.Body?.FirstOrDefault()?.Key != f.Key && !routeParameters.Contains(f.Key)))
                    {
                        if (parameterMapping.Parameter.FullTypeName is "string" or "System.String") // TODO
                        {
                            source.AppendLine($"if (!string.IsNullOrWhiteSpace({parameterMapping.Key}) && !queryString.ContainsKey(\"{parameterMapping.Key}\") )"); // TODO need to fix to allow multiple keys with same value as allowed in http querystring
                        }
                        else
                        {
                            source.AppendLine($"if ({parameterMapping.Key} != default && !queryString.ContainsKey(\"{parameterMapping.Key}\") )"); // TODO need to fix to allow multiple keys with same value as allowed in http querystring
                        }

                        source.AppendOpenCurlyBracketLine();
                        source.AppendLine($"queryString.Add(\"{parameterMapping.Key}\", {parameterMapping.Key}.ToString());");
                        source.AppendCloseCurlyBracketLine();
                    }
                }
                    
                source.AppendLine($"return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString({routeString}, queryString);");
                    
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
            
        if (config.ResponseWrapper.Enabled)
        {
            source.AppendLine("public class EmptyBodyException : ApplicationException");

            using (source.CreateBracket())
            {
                source.AppendLine("public EmptyBodyException(int statusCode, HttpResponseHeaders headers)");

                using (source.CreateBracket())
                {
                    source.AppendLine("StatusCode = statusCode;");
                    source.AppendLine("Headers = headers;");
                }
                
                source.AppendLine("public int StatusCode { get; set; }");
                source.AppendLine("public HttpResponseHeaders Headers { get; set; }");
            }
            
            source.AppendLine("public class UnsuccessfulException : ApplicationException");
            using (source.CreateBracket())
            {
                source.AppendLine("public UnsuccessfulException(int statusCode, HttpResponseHeaders headers)");

                using (source.CreateBracket())
                {
                    source.AppendLine("StatusCode = statusCode;");
                    source.AppendLine("Headers = headers;");
                }
                
                source.AppendLine("public int StatusCode { get; set; }");
                source.AppendLine("public HttpResponseHeaders Headers { get; set; }");
            }
            
            source.AppendLine("public class Response");

            using (source.CreateBracket())
            {

                if (config.ResponseWrapper.ExtractHeaders.Count > 0)
                {
                    source.AppendLine("public HttpResponseHeaders Headers {get; set;}");
                    source.AppendLine("public Response(HttpResponseHeaders headers)");

                    using (source.CreateBracket())
                    {
                        source.AppendLine("this.Headers = headers;");
                        source.AppendLine("if (headers != null)");
                        using (source.CreateBracket())
                        {
                            source.AppendLine("_populateValuesFromHeaders();");
                        }
                    }
                    
                    source.AppendLine("public Response(HttpStatusCode statusCode, HttpResponseHeaders headers)");

                    using (source.CreateBracket())
                    {
                        source.AppendLine("StatusCode = statusCode;");
                        source.AppendLine("this.Headers = headers;");
                        
                        source.AppendLine("if (headers != null)");
                        using (source.CreateBracket())
                        {
                            source.AppendLine("_populateValuesFromHeaders();");
                        }
                    }
                    
                    source.AppendLine("public void _populateValuesFromHeaders()");
                    using (source.CreateBracket())
                    {
                        foreach (var header in config.ResponseWrapper.ExtractHeaders)
                        {
                            source.AppendLine($"if (Headers != null && Headers.Contains(\"{header.Value}\") )");

                            using (source.CreateBracket())
                            {
                                source.AppendLine($"{header.Key} = Headers.GetValues(\"{header.Value}\");");
                            }
                        }
                    }
                }
                else
                {
                    source.AppendLine("public Response() {}");
                    source.AppendLine("public Response(HttpStatusCode statusCode)");

                    using (source.CreateBracket())
                    {
                        source.AppendLine("StatusCode = statusCode;");
                    }
                }
                
                source.AppendLine("public HttpStatusCode StatusCode { get; }");
                source.AppendLine("public bool Success => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);");
                
                if (config.ResponseWrapper.ExtractHeaders.Count > 0)
                {
                    foreach (var header in config.ResponseWrapper.ExtractHeaders)
                    {
                        source.AppendLine($"public IEnumerable<string> {header.Key} {{get; set; }} = [];");
                    }
                }
                
            }

            source.AppendLine("public class Response<T> : Response");

            using (source.CreateBracket())
            {
                source.AppendLine("public Response(HttpResponseHeaders headers) : base(headers) {}");

                if (config.ResponseWrapper.ExtractHeaders.Count > 0)
                {
                    source.AppendLine("public Response(HttpStatusCode statusCode, HttpResponseHeaders headers, Task<T?> data) : base(statusCode, headers)");
                    using (source.CreateBracket())
                    {
                        source.AppendLine("Data = data;");
                        source.AppendLine("this.Headers = headers;");
                        source.AppendLine("if (headers != null)");
                        using (source.CreateBracket())
                        {
                            source.AppendLine("_populateValuesFromHeaders();");
                        }
                    }
                }
                else
                {
                    source.AppendLine("public Response(HttpStatusCode statusCode, Task<T?> data) : base(statusCode)");
                    using (source.CreateBracket())
                    {
                        source.AppendLine("Data = data;");
                    }
                }

                source.AppendMultipleLines("""
                                           public Task<T?> Data { get; }

                                           public Task<T> SuccessData => Success ? Data ?? throw new  EmptyBodyException((int)StatusCode, Headers)
                                                                           : throw new UnsuccessfulException((int)StatusCode, Headers);
                                           """);
                source.AppendLine("public bool TryGetSuccessData([NotNullWhen(true)] out Task<T?> data)");

                using (source.CreateBracket())
                {
                    source.AppendLine("data = Data;");
                    source.AppendLine("return Success && data is not null;");
                }
            }
        }
    }
}