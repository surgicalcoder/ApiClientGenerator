using System.Net.Http;

namespace GoLive.Generator.ApiClientGenerator
{
    public record ActionRoute(string Name, HttpMethod Method, string Route, string? ReturnTypeName, bool hasCustomFormatter, ParameterMapping[] Mapping, ParameterMapping? Body);
}