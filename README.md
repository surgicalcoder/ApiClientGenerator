# ApiClientGenerator
A .NET 6.0 Strongly typed HttpApi Client Generator, mainly being used on a Blazor WebAssembly project. With this project, you can easily and quickly generate a strongly typed HttpClient, with some trimmings, and have it placed in your Blazor WebAssembly project.

 # How to use

Firstly, add the project from Nuget - [GoLive.Generator.ApiClientGenerator](https://www.nuget.org/packages/GoLive.Generator.ApiClientGenerator/), then add an AdditionalFile in your .csproj named "ApiClientGenerator.json", like so:

```
<ItemGroup>
     <AdditionalFiles Include="ApiClientGenerator.json" />
</ItemGroup>
```

There are a large number of options that are in the settings file:


```
{
  "OutputFiles":[
    "GeneratedApiClient.cs"
  ],
  "JSONAPIRepresentationFile":[
    "GeneratedApiClient.json"
  ],
  "Namespace": "GoLive.Generator.ApiClientGenerator.Tests.WebApi.Generated",
  "CustomDiscriminator": "",
  "Includes": [
    "GoLive.Generator.ApiClientGenerator"
  ],
  "PreAppendLines": [
    "// ReSharper disable All"
  ],
  "PostAppendLines": [
    "// ReSharper disable All"
  ] ,
  "HideUrlsRegex": [
    "^/_]*",
    "^/WeatherForecast/_]*",
    "^_]*"
  ],
  "RouteTemplate":"/api/{area:exists}/{controller=Home}/{action=Index}/{id?}",
  "ResponseWrapper" :{
    "Enabled": true,
    "ExtractHeaders":{
      "CorrelationId": "X-Correlation-Id"
    }
  },
  "OutputUrls": true,
  "OutputUrlsPostfix": "_Url",
  "OutputJSONSourceGenerator": true,
  "JSONSourceGeneratorSettings":{
    "Converters": [
      
    ]
  },
  "Properties":{
    "IgnoreGenericTypes" : [
      "System.Collections.Generic.List<T>"
    ],
    "IgnoreTypes":[
      "System.DateTime"
    ],
    "IgnoreThatHasAttribute":[
      "GoLive.Generator.ApiClientGenerator.Tests.WebApi.CustomAttributeModelBinderAttribute"
    ],
    "TransformType":[
      {
        "SourceType": "*",
        "DestinationType" : "System.String",
        "ContainsAttribute": "GoLive.Generator.ApiClientGenerator.Tests.WebApi.CustomAttributeModelBinder2Attribute"
      },
      {
        "SourceType": "*",
        "DestinationType" : "System.String",
        "ContainsAttribute": "GoLive.Generator.ApiClientGenerator.Tests.WebApi.CustomAttributeModelBinder3Attribute"
      }
    ]
  }
}
```

Explanation of options:


- OutputFiles - this is the file (or files) that will be Generated
- JSONAPIRepresentationFile (optional) - this will output a .json file that represents the API, if you want to use for an external process etc
- Namespace - Namespace of the generated classes
- CustomDiscriminator (optional) - If you want to use custom JSOn Dicriminators
- Includes (optional) - Any additional Include statements to be added at the top
- PreAppendLines / PostAppendLines (optional) - Any additional lines such as comments (to disable Resharper's processing)
- HideUrlsRegex (optional) - Regex patterns, if you want to hide any URLs from being outputted, such as secret admin APIs
- RouteTemplate - This is the route template that gets used to figure out the URLs
- ResponseWrapper (optional) - If you want responses to be wrapped with a Response object, and a few options for it (such as pulling out headers etc)
- OutputUrls / OutputUrlsPostfix (optional)- If you want to output pure methods that return URLs only
- OutputJSONSourceGenerator (optional) - If you want to output a JSON Source Generator 
- Properties (optional) - processing properties (ignoring properties, changing their type, ignoring of methods that have attributes etc)

### XML Comments on API

To get the XML comments pulling through on the API, you need to have:

```
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

On your project's csproj to make the comments visible to the source code generator.

If you don't want XML comments enabled, set the config variable of "DisableXMLComments" to true in the json file.

# Changelog

- Version 3.0 - changed the way the API generation works (uses a central route template as opposed to hard coded), changed json settings file (not backwards compatible but not that different)