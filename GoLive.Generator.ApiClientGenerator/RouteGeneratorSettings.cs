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

    public string PrefixUrl { get; set; }

    public bool UseResponseWrapper { get; set; }

    public bool OutputJSONSourceGenerator { get; set; }
    
    public bool OutputUrls { get; set; }
    public string OutputUrlsPrefix { get; set; }
    public string OutputUrlsPostfix { get; set; }

    public RouteGeneratorSettings_Properties Properties { get; set; } = new();
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