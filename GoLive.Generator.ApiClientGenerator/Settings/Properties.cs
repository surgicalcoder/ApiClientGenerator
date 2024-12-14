using System.Collections.Generic;

namespace GoLive.Generator.ApiClientGenerator.Settings;

public class Properties
{
    public List<string> IgnoreTypes { get; set; } = new();
    public List<string> IgnoreGenericTypes { get; set; }= new();
    public List<string> IgnoreThatHasAttribute { get; set; }= new();
    
    public Dictionary<string, string> IdempotencyRequired { get; set; } = new();
    public string IdempotencyGenerator { get; set; }

    public List<TransformTypeContainer> TransformType { get; set; } = new();
}