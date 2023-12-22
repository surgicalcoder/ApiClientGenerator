using System.Collections.Generic;

namespace GoLive.Generator.ApiClientGenerator;

public class Parameter(string FullTypeName, string? GenericTypeName, bool HasDefaultValue, object? DefaultValue, bool Nullable = false, List<string> Attributes=default)
{
    public string FullTypeName { get; set; } = FullTypeName;
    public string GenericTypeName { get; set; } = GenericTypeName;
    public bool HasDefaultValue { get; set; } = HasDefaultValue;
    public object DefaultValue { get; set; } = DefaultValue;
    public bool Nullable { get; set; } = Nullable;
    public List<string> Attributes { get; set; } = Attributes;
    
    public void Deconstruct(out string FullTypeName, out string? GenericTypeName, out bool HasDefaultValue, out object? DefaultValue, out bool Nullable, out List<string> Attributes)
    {
        FullTypeName = this.FullTypeName;
        GenericTypeName = this.GenericTypeName;
        HasDefaultValue = this.HasDefaultValue;
        DefaultValue = this.DefaultValue;
        Nullable = this.Nullable;
        Attributes = this.Attributes;
    }
}