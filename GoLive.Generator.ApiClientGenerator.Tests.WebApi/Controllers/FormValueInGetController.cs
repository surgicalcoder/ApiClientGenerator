using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

public class FormValueInGetController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> FormValueInQuerystringTest([FromForm] TestItem item)
    {
        return Ok(item);
    }
}

public class TestItem{
    public string TestContent { get; set; }
}