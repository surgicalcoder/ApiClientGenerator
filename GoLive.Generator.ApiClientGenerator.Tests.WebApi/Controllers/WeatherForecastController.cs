using GoLive.Generator.ApiClientGenerator.Tests.WebApi.Generated;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet("_secretUrl")]
    public async Task<ActionResult> SecretUrl()
    {
        ApiClient client = new ApiClient(new HttpClient());
//        client.User.GetUser2_Url()
        return Ok();
    }


    public async Task<ActionResult<byte[]>> GetBytes()
    {
        return new ActionResult<byte[]>(new byte[1]);
    }

    public WeatherForecast GetSingle(int Id)
    {
        return null;
    }
}