namespace GoLive.Generator.ApiClientGenerator.Settings;

public class JSONSourceGeneratorSettings
{
    public string[] Converters { get; set; } = [];
    public string PropertyNamingPolicy { get; set; } = "JsonKnownNamingPolicy.CamelCase";
    public bool AllowTrailingCommas { get; set; } = true;
    public string[] AdditionalOptions { get; set; } = [];
}