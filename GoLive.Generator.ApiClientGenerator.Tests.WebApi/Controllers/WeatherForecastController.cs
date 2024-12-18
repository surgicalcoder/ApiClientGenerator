using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

/// <summary>
/// This is a test of the XML Documentation feature for Weather Forecast Controller
/// </summary>
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
    public async Task<ActionResult> TestModelBinderDifferentName([CustomAttributeModelBinder2(Name = "OtherName")]string option)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> TestRemapTypeWithAnotherType(TimeSpan option)
    {
        return Ok();
    }    
    
    [HttpPost]
    public async Task<ActionResult> TestRemapTypeWithAnotherType2([CustomAttributeModelBinder2]TimeSpan option)
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

    /// <summary>
    /// This is a secret URL! This shouldn't be outputted
    /// </summary>
    /// <returns>A 400 error</returns>
    [HttpGet("_secretUrl")]
    public async Task<ActionResult> SecretUrl()
    {
        return Ok();
    }

    /// <summary>
    /// This is a test of the XML Documentation feature for UrlWithParametersFromRoute
    /// </summary>
    /// <param name="Input1">This is the string of the input param1</param>
    /// <param name="Input2">This is the string of the input param2</param>
    /// <returns></returns>
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