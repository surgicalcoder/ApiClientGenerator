using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class InheritingUser2Controller : UserController
{
      public override string OverrideTest(string Id)
      {
            return "new";
      }
}


public class InheritingTwo : InheritingUser2Controller
{
      [HttpGet]
      [Route("{Page:int}/{PageSize:int}")]
      public virtual async Task<ActionResult> GetPagedApiTest(int Page = 1, string Filter = null, int PageSize = 20)
      {
            return Ok();
      }
}