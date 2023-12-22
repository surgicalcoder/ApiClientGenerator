using System.Collections.Generic;

namespace GoLive.Generator.ApiClientGenerator;

public record Parameter(string FullTypeName, string? GenericTypeName, bool HasDefaultValue, object? DefaultValue, bool Nullable = false, List<string> Attributes=default);