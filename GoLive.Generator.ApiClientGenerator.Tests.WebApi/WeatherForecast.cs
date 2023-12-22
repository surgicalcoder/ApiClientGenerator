using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}

public class CustomAttributeModelBinderAttribute : Attribute, IModelNameProvider, IBinderTypeProviderMetadata
{
    public string? Name { get; set; }
    public BindingSource? BindingSource { get; set; }
    public Type? BinderType { get; set; }
}