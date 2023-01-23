using System.Net.Http;

namespace GoLive.Generator.ApiClientGenerator
{
    public record ActionRoute(string Name, HttpMethod Method, string Route, string? ReturnTypeName, bool ReturnTypeStruct, 
        bool hasCustomFormatter, ParameterMapping[] Mapping, ParameterMapping? Body);
}