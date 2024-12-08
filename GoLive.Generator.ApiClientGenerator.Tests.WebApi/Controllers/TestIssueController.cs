using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

public class TestIssueController : ControllerBase
{
    public async Task<ActionResult> Get(string Id = "")
    {
        return Ok();
    }
}