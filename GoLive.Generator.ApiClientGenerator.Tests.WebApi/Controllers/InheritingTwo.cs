using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

public class InheritingTwo : InheritingUser2Controller
{
    [HttpGet]
    [Route("{Page:int}/{PageSize:int}")]
    public virtual async Task<ActionResult> GetPagedApiTest(int Page = 1, string Filter = null, int PageSize = 20)
    {
        return Ok();
    }
      
    [HttpGet]
    [Route("/ThisIsTestTwo/{Page:int}")]
    public virtual async Task<ActionResult> GetApiTest2(int Page = 1)
    {
        return Ok();
    }
      
      
}