# ApiClientGenerator
A .NET 6.0 Strongly typed HttpApi Client Generator, mainly being used on a Blazor WebAssembly project. With this project, you can easily and quickly generate a strongly typed HttpClient, with some trimmings, and have it placed in your Blazor WebAssembly project.

 # How to use

Firstly, add the project from Nuget - [GoLive.Generator.ApiClientGenerator](https://www.nuget.org/packages/GoLive.Generator.ApiClientGenerator/), then add an AdditionalFile in your .csproj named "ApiClientGenerator.json", like so:

```
<ItemGroup>
     <AdditionalFiles Include="ApiClientGenerator.json" />
</ItemGroup>
```

Once that's done, add the settings file and change as required:


```
{
  "OutputFile": "GeneratedApiClient.cs",
  "Namespace": "GoLive.Generator.ApiClientGenerator.Tests.WebApi.Generated",
  "CustomDiscriminator": "CustomDiscriminatorCreate()",
  "Includes": [
    "GoLive.Generator.ApiClientGenerator"
  ],
  "PreAppendLines": [
    "// ReSharper disable All"
  ],
  "PostAppendLines": [
    "// ReSharper disable All"
  ] 
}
```

For `OutputFile` (or `OutputFiles` if you want multiple), the path will be calculated as relative, so you can put in `..\WebAssembly\File.cs`