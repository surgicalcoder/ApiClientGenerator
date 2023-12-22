using System.Collections.Generic;
using System.Net.Http;

namespace GoLive.Generator.ApiClientGenerator;

public record ActionRoute(string Name, HttpMethod Method, string Route, bool RouteSetByAttributes, string? ReturnTypeName, bool ReturnTypeStruct, 
    bool hasCustomFormatter, List<ParameterMapping> Mapping, List<ParameterMapping> Body);