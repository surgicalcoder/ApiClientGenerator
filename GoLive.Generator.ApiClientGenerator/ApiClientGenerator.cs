﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using GoLive.Generator.ApiClientGenerator.Model;
using GoLive.Generator.ApiClientGenerator.Routing;
using GoLive.Generator.ApiClientGenerator.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GoLive.Generator.ApiClientGenerator;

[Generator]
public class ApiClientGenerator : IIncrementalGenerator
{
    private const string TASK_FQ = "global::System.Threading.Tasks.Task";
    private const string IFormFile_Q = "Microsoft.AspNetCore.Http.IFormFile";
        
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
        source.AppendLine("using System;");
        source.AppendLine("using System.Threading.Tasks;");
        source.AppendLine("using System.Net.Http.Json;");
        source.AppendLine("using System.Collections.Generic;");
        source.AppendLine("using System.Threading;");
        source.AppendLine("using System.Diagnostics.CodeAnalysis;");
        source.AppendLine("using System.Text.Json.Serialization.Metadata;");
        source.AppendLine("using Microsoft.Extensions.Primitives;");
        source.AppendLine("using Microsoft.AspNetCore.Http;");

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
                    jsonGenOptions.Add("PropertyNamingPolicy", config.JSONSourceGeneratorSettings.PropertyNamingPolicy);
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

                source.Append(string.Join(",", jsonGenOptions.Select(r => $"{r.Key} = {r.Value}")));

                if (config.JSONSourceGeneratorSettings.AdditionalOptions is { Length: > 0 })
                {
                    source.Append(", ");
                    source.Append(string.Join(", ", config.JSONSourceGeneratorSettings.AdditionalOptions));
                }
            }

            source.Append(")]\n");

            var returnTypes = orderedControllerRoutes.SelectMany(e => e.Actions.Select(f => f.ReturnTypeName)).Distinct()
                .Where(r => !string.IsNullOrWhiteSpace(r) && r.StartsWith("global::")).Except(ignoreTypesList);

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

        if (config.JSONAPIRepresentationFile?.Count > 0)
        {
            GenerateJSONRepresentation(controllerRoutes, config);
        }

        if (config.OutputFiles == null || config.OutputFiles.Count == 0)
        {
            context.AddSource("GeneratedApiClient", source.ToString());
        }
        else
        {
            SaveSourceToFile(config, source);
        }
    }

    private static void GenerateJSONRepresentation(ImmutableArray<ControllerRoute> controllerRoutes, RouteGeneratorSettings config)
    {
        var jsonOutput = System.Text.Json.JsonSerializer.Serialize(controllerRoutes, new JsonSerializerOptions { WriteIndented = true });
        foreach (var file in config.JSONAPIRepresentationFile)
        {
            File.WriteAllText(file, jsonOutput);
        }
    }

    private static void SaveSourceToFile(RouteGeneratorSettings config, SourceStringBuilder source)
    {
        var content = source.ToString();

        if (config.OutputFiles is not { Count: > 0 })
        {
            return;
        }

        foreach (var configOutputFile in config.OutputFiles)
        {
            File.WriteAllText(configOutputFile, content);
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
        
        if (config.JSONAPIRepresentationFile is { Count: > 0 })
        {
            config.JSONAPIRepresentationFile = config.JSONAPIRepresentationFile.Select(e => Path.GetFullPath(Path.Combine(configFileDirectory, e))).ToList();
        }

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

        
        if (!string.IsNullOrWhiteSpace(controllerRoute.XmlComments) && !config.DisableXMLComments)
        {
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(controllerRoute.XmlComments);
            var memberNode = xmlDoc.SelectSingleNode("//member");
            if (memberNode != null)
            {
                foreach (var line in memberNode.InnerXml.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
                {
                    source.AppendLine($"/// {line}");
                }
            }
        }
        
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
            
            bool containsFileUpload = action.Mapping.Any(f => f.Parameter.FullTypeName == IFormFile_Q);

            if (containsFileUpload) 
            {
                parameterList = string.Join(", ",
                    action.Mapping.Select(m =>
                        m.Parameter.FullTypeName == IFormFile_Q
                            ? $"System.Net.Http.MultipartFormDataContent{(m.Parameter.Nullable ? "?" : "")} multiPartContent"
                            : $"{m.Parameter.FullTypeName} {m.Key} {GetDefaultValue(m.Parameter)}"));
                
                
            }
            
            var mappingsWithoutFile = action.Mapping.Where(f => f.Parameter.FullTypeName != IFormFile_Q).ToList();

            string useCustomFormatter = config.CustomDiscriminator;
            
            URLTemplate urlTemplate;
            
            if (action.RouteSetByAttributes)
            {
                urlTemplate = URLTemplate.Parse(action.Route);
            }
            else
            {
                urlTemplate = URLTemplate.Parse(config.RouteTemplate);
            }

            var routeValues = new CaseInSensitiveDictionary
            {
                { "Controller", controllerRoute.Name },
                { "Action", action.Name }
            };

            if (controllerRoute.Area != null && !string.IsNullOrWhiteSpace(controllerRoute.Area))
            {
                routeValues.Add("Area", controllerRoute.Area);
            }
            
            CaseInSensitiveDictionary actionValues = new(); 
            
            if (controllerRoute.Area != null && !string.IsNullOrWhiteSpace(controllerRoute.Area))
            {
                actionValues.Add("Area", controllerRoute.Area); // TODO replace Area with const
            }
            
            actionValues.Add("Controller", controllerRoute.Name);
            actionValues.Add("Action", action.Name);
            
            
            foreach (var (key, parameter) in action.Mapping
                .Where(m => !string.Equals(m.Parameter.FullTypeName, IFormFile_Q, StringComparison.InvariantCultureIgnoreCase))
                .Where(m => 
                    urlTemplate.Segments.Any(s => string.Equals(s.Parameter, m.Key, StringComparison.InvariantCultureIgnoreCase) 
                                                  /*|| 
                                                  (!m.Parameter.Nullable && !m.Parameter.HasDefaultValue)*/
                    )
                    ))
            {
                actionValues.Add(key, $"{{{key}}}");
            }
            
            string routeValue = urlTemplate.Render(actionValues);

            if (config.HideUrlsRegex is { Count: > 0 })
            {
                if (config.HideUrlsRegex.Any(e => Regex.IsMatch(routeValue, e)))
                {
                    continue;
                }
            }
            
            var routeString = $"$\"{routeValue}{{queryString}}\"";
                
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

            
            if (!string.IsNullOrWhiteSpace(action.XmlComments) && !config.DisableXMLComments)
            {
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(action.XmlComments);
                var memberNode = xmlDoc.SelectSingleNode("//member");
                if (memberNode != null)
                {
                    foreach (var line in memberNode.InnerXml.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
                    {
                        source.AppendLine($"/// {line}");
                    }
                }
            }
            
            
            source.AppendLine(string.IsNullOrWhiteSpace(parameterList)
                ? $"public async {returnType} {action.Name}(QueryString queryString = default, CancellationToken _token = default {jsonTypeInfoMethodParameter})"
                : $"public async {returnType} {action.Name}({parameterList}, QueryString queryString = default, CancellationToken _token = default {jsonTypeInfoMethodParameter})");

            source.AppendOpenCurlyBracketLine();
            
            if (config.OutputJSONSourceGenerator && (!string.IsNullOrWhiteSpace(action.ReturnTypeName) && action.ReturnTypeName != TASK_FQ && !byteReturnType))
            {
                source.AppendLine("if (_typeInfo == default)");

                using (source.CreateBracket())
                {
                    source.AppendLine($"_typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof({action.ReturnTypeName})) as JsonTypeInfo<{action.ReturnTypeName}>;");
                }
            }

            foreach (var parameterMapping in action.Mapping.Where(f => action.Body?.FirstOrDefault()?.Key != f.Key 
                                                                       && !actionValues.ContainsKey(f.Key) && !urlTemplate.Segments.Any(r=> string.Equals(r.Parameter, f.Key, StringComparison.InvariantCultureIgnoreCase) 
                                                                                                        && r.Restriction == URLTemplateSegmentKnownRestrictions.Optional)))
            {
                if (parameterMapping.Parameter.SpecialType == SpecialType.System_String)
                {
                    source.AppendLine($"if (!string.IsNullOrWhiteSpace({parameterMapping.Key}))");
                }
                else
                {
                    source.AppendLine($"if ({parameterMapping.Key} != default)");
                }

                using (source.CreateBracket())
                {
                    source.AppendLine($"queryString = queryString.Add(\"{parameterMapping.Key}\", {parameterMapping.Key}.ToString());"); 
                }
            }


            action.CalculatedURL = routeValue;
            
            var HttpRequestHttpMethod = action.Method.Method switch
            {
                "GET" => "HttpMethod.Get",
                "POST" => "HttpMethod.Post",
                "PUT" => "HttpMethod.Put",
                "DELETE" => "HttpMethod.Delete",
                "PATCH" => "new HttpMethod(\"PATCH\")",
                "HEAD" => "HttpMethod.Head",
                "OPTIONS" => "HttpMethod.Options",
                "TRACE" => "HttpMethod.Trace",
                _ => $"new HttpMethod(\"{action.Method.Method}\")"
            };
            
            source.AppendLine($"using var request = new HttpRequestMessage({HttpRequestHttpMethod}, {routeString});");

            if (containsFileUpload)
            {
                source.AppendLine("request.Content = multiPartContent;");
            }
            else if (action.Body is { Count: > 0 })
            {
                source.AppendLine($"request.Content = JsonContent.Create({action.Body.FirstOrDefault().Key});");
            }
            
            
            if (config.Properties.IdempotencyRequired is {Count: > 0})
            {
                string? idempotencyValue = string.Empty;

                bool actionHasIdempotency = action.AllAttributes.Any(attr => config.Properties.IdempotencyRequired.TryGetValue(attr, out idempotencyValue));

                bool controllerHasIdempotency = !actionHasIdempotency && controllerRoute.AllAttributes.Any(attr => config.Properties.IdempotencyRequired.TryGetValue(attr, out idempotencyValue));

                if (actionHasIdempotency || controllerHasIdempotency && !string.IsNullOrWhiteSpace(idempotencyValue))
                {
                    source.AppendLine(!string.IsNullOrWhiteSpace(config.Properties.IdempotencyGenerator) ? $"var idempotencyKey = {config.Properties.IdempotencyGenerator};" : "var idempotencyKey = Guid.NewGuid().ToString();");
                    source.AppendLine($"request.Headers.Add(\"{idempotencyValue}\", idempotencyKey);");
                }
            }
            
            string callStatement = "await _client.SendAsync(request, _token);";

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
                        source.AppendLine("if (_typeInfo != default)");

                        using (source.CreateBracket())
                        {
                            source.AppendLine($"return new Response<{action.ReturnTypeName}>(result.StatusCode, result.Headers, ({readValue} ?? Task.FromResult<{nullableReturnType}>(default)));"); // TODO store value for repeated use
                        }
                        source.AppendLine("else");
                        using (source.CreateBracket())
                        {
                            source.AppendLine($"return new Response<{action.ReturnTypeName}>(result.StatusCode, result.Headers, ({readValueWithoutJsonTypeInformation} ?? Task.FromResult<{nullableReturnType}>(default)));"); // TODO store value for repeated use
                        }
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

                var methodParameterMappings = action.Mapping
                    .Where(f => f.Parameter.FullTypeName != IFormFile_Q)
                    .Where(e => action.Method == HttpMethod.Get || 
                                (action.Method != HttpMethod.Get && !action.Body.Any(b => string.Equals(b.Key, e.Key, StringComparison.InvariantCultureIgnoreCase))))
                    .ToList();

                if (methodParameterMappings.Any())
                {
                    secondParamList.AddRange(methodParameterMappings.Select(parameterMapping => $"{parameterMapping.Parameter.FullTypeName} {parameterMapping.Key} {GetDefaultValue(parameterMapping.Parameter)}"));
                }
                
                /*foreach (var (key, parameter) in methodParameterMappings)
                {
                    source.AppendLine($"// {key} {parameter.FullTypeName}");
                }*/
                
                if (methodParameterMappings.Any())
                {
                    string parameterListWithoutFile = string.Join(", ", methodParameterMappings.Select(m => $"{m.Parameter.FullTypeName} {m.Key} {GetDefaultValue(m.Parameter)}"));
                    source.AppendLine($"public string {config.OutputUrlsPrefix}{action.Name}{config.OutputUrlsPostfix} ({string.Join(",", parameterListWithoutFile)}, QueryString queryString = default)");
                }
                else
                {
                    source.AppendLine($"public string {config.OutputUrlsPrefix}{action.Name}{config.OutputUrlsPostfix} (QueryString queryString = default)");
                }
                source.AppendOpenCurlyBracketLine();
   
                if (methodParameterMappings != null && methodParameterMappings.Any())
                { 
                    foreach (var parameterMapping in methodParameterMappings.Where(r=> !actionValues.ContainsKey(r.Key)))
                    {
                        source.AppendLine($"queryString = queryString.Add(\"{parameterMapping.Key}\", {parameterMapping.Key}.ToString());");
                    }
                }
                    
                source.AppendLine($"return {routeString};");
                    
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