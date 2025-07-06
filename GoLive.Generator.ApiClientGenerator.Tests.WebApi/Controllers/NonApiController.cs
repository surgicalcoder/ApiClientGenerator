using System.ComponentModel.DataAnnotations;
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
    
    [HttpGet]
    public async Task<ActionResult> TestWithAllowedValues(string Id, [AllowedValues("start", "stop", "kill", "restart")]string DesiredState)
    {
        return Ok();
    }
    
        
    [HttpGet]
    public async Task<ActionResult> TestWithAllowedValuesButNullable(string Id, [AllowedValues("start", "stop", "kill", "restart")]string? DesiredState)
    {
        return Ok();
    }
}