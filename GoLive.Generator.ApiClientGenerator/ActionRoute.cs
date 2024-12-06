using System.Collections.Generic;
using System.Net.Http;

namespace GoLive.Generator.ApiClientGenerator;

public class ActionRoute(string Name, string FullMethodName, HttpMethod Method, string Route, bool RouteSetByAttributes, string? ReturnTypeName, bool ReturnTypeStruct, 
    bool hasCustomFormatter, List<ParameterMapping> Mapping, List<ParameterMapping> Body, string XmlComments)
{
    public string Name { get; init; } = Name;
    public string FullMethodName { get; init; } = FullMethodName;
    public HttpMethod Method { get; init; } = Method;
    public string Route { get; init; } = Route;
    public bool RouteSetByAttributes { get; init; } = RouteSetByAttributes;
    public string ReturnTypeName { get; init; } = ReturnTypeName;
    public bool ReturnTypeStruct { get; init; } = ReturnTypeStruct;
    public bool hasCustomFormatter { get; init; } = hasCustomFormatter;
    public List<ParameterMapping> Mapping { get; init; } = Mapping;
    public List<ParameterMapping> Body { get; init; } = Body;
    public string XmlComments { get; init; } = XmlComments;
    public string CalculatedURL { get; set; }
    public void Deconstruct(out string Name, out string FullMethodName, out HttpMethod Method, out string Route, out bool RouteSetByAttributes, out string? ReturnTypeName, out bool ReturnTypeStruct, out bool hasCustomFormatter, out List<ParameterMapping> Mapping, out List<ParameterMapping> Body, out string XmlComments)
    {
        Name = this.Name;
        FullMethodName = this.FullMethodName;
        Method = this.Method;
        Route = this.Route;
        RouteSetByAttributes = this.RouteSetByAttributes;
        ReturnTypeName = this.ReturnTypeName;
        ReturnTypeStruct = this.ReturnTypeStruct;
        hasCustomFormatter = this.hasCustomFormatter;
        Mapping = this.Mapping;
        Body = this.Body;
        XmlComments = this.XmlComments;
    }
}