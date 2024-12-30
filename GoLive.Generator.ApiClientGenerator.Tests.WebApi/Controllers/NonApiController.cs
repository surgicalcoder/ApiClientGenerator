using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

public class NonApiController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> TestModelBinderDifferentNameUnderId([CustomAttributeModelBinder2(Name = "Id")]TestModel input)
    {
        return Ok();
    }
      
    [HttpGet]
    public async Task<ActionResult> TestModelBinderDifferentNameUnderId3([CustomAttributeModelBinder3(Name = "Id")]TestModel input)
    {
        return Ok();
    }
}