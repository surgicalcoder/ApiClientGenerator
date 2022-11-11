﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GoLive.Generator.ApiClientGenerator
{
    [Generator]
    public class ApiClientGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var config = LoadConfig(context);

            var source = new SourceStringBuilder();

            var compilation = context.Compilation;

            if (config.PreAppendLines != null && config.PreAppendLines.Count > 0)
            {
                config.PreAppendLines.ForEach(source.AppendLine);
            }


            source.AppendLine("using System.Net.Http;");
            source.AppendLine("using System.Threading.Tasks;");
            source.AppendLine("using System.Net.Http.Json;");
            source.AppendLine("using System.Collections.Generic;");
            source.AppendLine("using System.Threading;");

            if (config.Includes != null && config.Includes.Any())
            {
                foreach (var s in config.Includes)
                {
                    source.AppendLine($"using {s};");
                }
            }

            source.AppendLine();

            if (!string.IsNullOrWhiteSpace(config.Namespace))
            {
                source.AppendLine($"namespace {config.Namespace}");
            }
            else
            {
                source.AppendLine("namespace AutogeneratedClient");
            }


            source.AppendOpenCurlyBracketLine();
            var controllerRoutes = compilation.SyntaxTrees
                .Select(t => compilation.GetSemanticModel(t))
                .Select(Scanner.ScanForControllers)
                .SelectMany(c => c)
                .ToArray();

            SetUpApiClient(controllerRoutes, source, compilation);

            foreach (var route in controllerRoutes)
            {
                SetUpSingleApi(config, route, source, compilation);
            }

            source.AppendCloseCurlyBracketLine();

            if (config.PostAppendLines != null && config.PostAppendLines.Count > 0)
            {
                config.PostAppendLines.ForEach(source.AppendLine);
            }

            if (config == null || (string.IsNullOrWhiteSpace(config.OutputFile) && (config.OutputFiles == null || config.OutputFiles.Count == 0)))
            {
                context.AddSource("GeneratedApiClient", source.ToString());
            }
            else
            {
                if (config.OutputFiles != null && config.OutputFiles.Count > 0)
                {
                    foreach (var configOutputFile in config.OutputFiles)
                    {
                        if (File.Exists(configOutputFile))
                        {
                            File.Delete(configOutputFile);
                        }

                        File.WriteAllText(configOutputFile, source.ToString());
                    }
                }
                else
                {
                    if (File.Exists(config.OutputFile))
                    {
                        File.Delete(config.OutputFile);
                    }

                    File.WriteAllText(config.OutputFile, source.ToString());
                }
            }
        }

        private RouteGeneratorSettings LoadConfig(GeneratorExecutionContext context)
        {
            var configFilePath = context.AdditionalFiles.FirstOrDefault(e => e.Path.EndsWith("ApiClientGenerator.json"));

            if (configFilePath == null)
            {
                return null;
            }

            var jsonString = File.ReadAllText(configFilePath.Path);
            var config = JsonSerializer.Deserialize<RouteGeneratorSettings>(jsonString);
            var configFileDirectory = Path.GetDirectoryName(configFilePath.Path);

            var fullPath = Path.Combine(configFileDirectory, config.OutputFile);
            config.OutputFile = Path.GetFullPath(fullPath);

            if (config.OutputFiles != null && config.OutputFiles.Count > 0)
            {
                config.OutputFiles = config.OutputFiles.Select(e => Path.GetFullPath(Path.Combine(configFileDirectory, e))).ToList();
            }

            return config;
        }

        private void SetUpSingleApi(RouteGeneratorSettings config, ControllerRoute controllerRoute, SourceStringBuilder source, Compilation compilation)
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

            className = className + "Client";

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
                var returnType = action.ReturnTypeName != null
                    ? $"Task<{action.ReturnTypeName}>"
                    : "Task";
                
                bool byteReturnType = false || action.ReturnTypeName == "byte[]";
                Console.WriteLine(action.ReturnTypeName);

                var parameterList = string.Join(", ", action.Mapping.Select(m => $"{m.Parameter.FullTypeName} {m.Key} {GetDefaultValue(m.Parameter)}"));

                bool containsFileUpload = action.Mapping.Any(f => f.Parameter.FullTypeName == "Microsoft.AspNetCore.Http.IFormFile");

                if (containsFileUpload)
                {
                    parameterList = string.Join(", ", action.Mapping.Select(m => m.Parameter.FullTypeName == "Microsoft.AspNetCore.Http.IFormFile" ? $"System.Net.Http.MultipartFormDataContent multiPartContent" : $"{m.Parameter.FullTypeName} {m.Key} {GetDefaultValue(m.Parameter)}"));
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

                routeValue = routeValue.Replace("*", ""); // TODO - to remove greedy url params

                var routeString = $"$\"{routeValue}\"";

                if (config.HideUrlsRegex is { Count: > 0 })
                {
                    if (config.HideUrlsRegex.Any(e => Regex.IsMatch(routeValue, e)))
                    {
                        continue;
                    }
                }
                
                source.AppendLine();

                if (string.IsNullOrWhiteSpace(parameterList))
                {
                    source.AppendLine($"public async {returnType} {action.Name}(CancellationToken _token = default)");
                }
                else
                {
                    source.AppendLine($"public async {returnType} {action.Name}({parameterList}, CancellationToken _token = default)");
                }
                
                source.AppendOpenCurlyBracketLine();

                if (action.Mapping.Any(f => f.Key.ToLower() != "id" && action.Body?.Key != f.Key))
                {
                    source.AppendLine("Dictionary<string, string> queryString=new();");

                    foreach (var parameterMapping in action.Mapping.Where(f => f.Key != "Id" && action.Body?.Key != f.Key))
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
                    source.AppendLine(callStatement);
                }
                else
                {
                    source.AppendLine($"var result = {callStatement}");

                    if (byteReturnType)
                    {
                        source.AppendLine("return await result.Content.ReadAsByteArrayAsync();");
                    }
                    else if (string.IsNullOrWhiteSpace(useCustomFormatter))
                    {
                        source.AppendLine($"return await result.Content.ReadFromJsonAsync<{action.ReturnTypeName}>();");
                    }
                    else
                    {
                        source.AppendLine($"return await result.Content.ReadFromJsonAsync<{action.ReturnTypeName}>({useCustomFormatter});");
                    }
                    
                }

                source.AppendCloseCurlyBracketLine();
            }

            source.AppendCloseCurlyBracketLine();
        }

        private string GetDefaultValue(Parameter argParameter)
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

        private static void SetUpApiClient(IEnumerable<ControllerRoute> routes, SourceStringBuilder source, Compilation compilation)
        {
            source.AppendLine();
            source.AppendLine("public class ApiClient");
            source.AppendOpenCurlyBracketLine();

            source.AppendLine("private readonly HttpClient _client;");
            source.AppendLine();

            source.AppendLine("public ApiClient(HttpClient client)");
            source.AppendOpenCurlyBracketLine();

            source.AppendLine("_client = client;");

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
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
        }
    }
}