using System;
using System.Collections.Generic;

namespace GoLive.Generator.ApiClientGenerator;

public class RouteGeneratorSettings
{
    public string OutputFile { get; set; }
    public List<string> OutputFiles { get; set; }
    public List<string> Includes { get; set; }

    public string CustomDiscriminator { get; set; }
    public string Namespace { get; set; }

    public List<string> PreAppendLines { get; set; }
    public List<string> PostAppendLines { get; set; }
        
    public List<string> HideUrlsRegex { get; set; }

    public string RouteTemplate { get; set; }

    public RouteGeneratorSettings_ResponseWrapperSettings ResponseWrapper { get; set; } = new();

    public bool OutputJSONSourceGenerator { get; set; }

    public RouteGeneratorSettings_JSONSourceGeneratorSettings JSONSourceGeneratorSettings { get; set; } = new();
    
    public bool OutputUrls { get; set; }
    public string OutputUrlsPrefix { get; set; }
    public string OutputUrlsPostfix { get; set; }

    public string JsonOutputFilename { get; set; }

    public RouteGeneratorSettings_Properties Properties { get; set; } = new();
}

public class RouteGeneratorSettings_ResponseWrapperSettings
{
    public bool Enabled { get; set; }
    public Dictionary<string, string> ExtractHeaders { get; set; } = new();
}

public class RouteGeneratorSettings_JSONSourceGeneratorSettings
{
    public string[] Converters { get; set; } = [];
    public string PropertyNamingPolicy { get; set; } = "JsonKnownNamingPolicy.CamelCase";
    public bool AllowTrailingCommas { get; set; } = true;
    public string[] AdditionalOptions { get; set; } = [];
}

public class RouteGeneratorSettings_Properties
{
    public List<string> IgnoreTypes { get; set; } = new();
    public List<string> IgnoreGenericTypes { get; set; }= new();
    public List<string> IgnoreThatHasAttribute { get; set; }= new();

    public List<TransformTypeContainer> TransformType { get; set; } = new();
}
public class TransformTypeContainer
{
    public string SourceType { get; set; }
    public string DestinationType { get; set; }
    public string ContainsAttribute { get; set; }
}