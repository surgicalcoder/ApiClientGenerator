using System.Diagnostics;

namespace GoLive.Generator.ApiClientGenerator.Routing;

[DebuggerDisplay("{Raw}")]
public class URLTemplateSegment
{
    public string Raw { get; set; }
    public URLTemplateReplaceableElement? BuiltInReplaceable { get; set; }
    public string Parameter { get; set; }
    public string DefaultValue { get; set; }
    public string Restriction { get; set; }
    public bool IsCatchall { get; set; }
}