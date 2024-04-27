using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace GoLive.Generator.ApiClientGenerator;

public class Parameter
{
    public Parameter(string FullTypeName, string? GenericTypeName, bool HasDefaultValue, object? DefaultValue, bool Nullable = false, List<string> Attributes=default, SpecialType SpecialType = SpecialType.None)
    {
        this.FullTypeName = FullTypeName;
        this.GenericTypeName = GenericTypeName;
        this.HasDefaultValue = HasDefaultValue;
        this.DefaultValue = DefaultValue;
        this.Nullable = Nullable;
        this.Attributes = Attributes;
        this.SpecialType = SpecialType;
    }
    public string FullTypeName { get; set; }
    public string GenericTypeName { get; set; }
    public bool HasDefaultValue { get; set; }
    public object DefaultValue { get; set; }
    public bool Nullable { get; set; }
    public List<string> Attributes { get; set; }
    public SpecialType SpecialType { get; set; }

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