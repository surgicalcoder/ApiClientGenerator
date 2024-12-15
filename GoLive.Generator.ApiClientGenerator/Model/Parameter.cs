using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace GoLive.Generator.ApiClientGenerator.Model;

public class Parameter
{
    public Parameter(string FullTypeName, string? GenericTypeName, bool HasDefaultValue, object? DefaultValue, bool Nullable = false, List<string> Attributes=default, SpecialType SpecialType = SpecialType.None, string[] allowedStringValues = null)
    {
        this.FullTypeName = FullTypeName;
        this.GenericTypeName = GenericTypeName;
        this.HasDefaultValue = HasDefaultValue;
        this.DefaultValue = DefaultValue;
        this.AllowedStringValues = allowedStringValues;
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
    public string[] AllowedStringValues { get; set; }
}