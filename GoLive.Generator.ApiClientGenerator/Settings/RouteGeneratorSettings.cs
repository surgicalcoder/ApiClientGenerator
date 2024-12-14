using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoLive.Generator.ApiClientGenerator.Settings;

public class RouteGeneratorSettings
{
    [JsonConverter(typeof(StringOrArrayJsonConverter))]
    public List<string> OutputFiles { get; set; }
    public List<string> Includes { get; set; }

    public string CustomDiscriminator { get; set; }
    public string Namespace { get; set; }

    public List<string> PreAppendLines { get; set; }
    public List<string> PostAppendLines { get; set; }
        
    public List<string> HideUrlsRegex { get; set; }

    public string RouteTemplate { get; set; }

    public ResponseWrapperSettings ResponseWrapper { get; set; } = new();

    public bool OutputJSONSourceGenerator { get; set; }

    public JSONSourceGeneratorSettings JSONSourceGeneratorSettings { get; set; } = new();
    
    public bool OutputUrls { get; set; }
    public string OutputUrlsPrefix { get; set; }
    public string OutputUrlsPostfix { get; set; }
    
    public bool DisableXMLComments { get; set; }

    [JsonConverter(typeof(StringOrArrayJsonConverter))]
    public List<string> JSONAPIRepresentationFile { get; set; }

    public Properties Properties { get; set; } = new();
}