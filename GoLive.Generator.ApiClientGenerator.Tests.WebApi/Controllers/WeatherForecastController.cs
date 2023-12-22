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

    [HttpPost]
    public async Task<ActionResult> TestIgnoreGenericParmaeter(List<string> options, string optionNotRemoved)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> TestIgnoreNormalParameter(DateTime option, string optionNotRemoved)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> TestIgnoreWithCustomAttribute([CustomAttributeModelBinder]string option, string optionNotRemoved)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> TestRemapTypeWithAnotherType(TimeSpan option)
    {
        return Ok();
    }

    /*public async Task<ActionResult<string>> TestIgnore(List<string> options)
    {
        return Ok("yes");
    }*/

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
        return Ok();
    }

    [HttpGet]
    [Route("/[controller]/UrlWithParametersFromRoute/{Input1}/{Input2}")]
    public async Task<ActionResult> UrlWithParametersFromRoute(string Input1, string Input2)
    {
        return Ok();
    }

    [HttpGet]
    [Route("/[controller]/UrlWithParametersFromRoute2/{Input1}/{Input2}")]
    public async Task<ActionResult> UrlWithParametersFromRoute2(string Input1, string Input2, string Input3)
    {
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
    
    public WeatherForecast GetSingleFromServiceExample([FromServicesAttribute]int Id)
    {
        return null;
    }

    [HttpPost]
    public async Task<ActionResult> FormUploadTest1(IFormFile formFile)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult> FormUploadTest2(IFormFile? formFile)
    {
        return Ok();
    }

    public async Task TaskIssue()
    {
    }
}