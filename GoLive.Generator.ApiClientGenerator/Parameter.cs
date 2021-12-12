namespace GoLive.Generator.ApiClientGenerator
{
    public record Parameter(string FullTypeName, bool HasDefaultValue, object? DefaultValue);
}