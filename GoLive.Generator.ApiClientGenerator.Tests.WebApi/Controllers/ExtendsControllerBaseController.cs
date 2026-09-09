using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

[ApiController]
public class ExtendsControllerBaseController : ControllerBase
{
    [HttpGet]
    public string GetByName(string name)
    {
        return "ok";
    }

    [HttpDelete("{id}")]
    public string DeleteById(string id)
    {
        return "deleted";
    }
}