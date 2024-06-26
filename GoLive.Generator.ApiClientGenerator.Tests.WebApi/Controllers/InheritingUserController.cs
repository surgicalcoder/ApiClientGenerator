using System.Threading.Tasks;
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
      
      [HttpGet]
      [Route("/ThisIsTestTwo/{Page:int}")]
      public virtual async Task<ActionResult> GetApiTest2(int Page = 1)
      {
            return Ok();
      }
      
      
}

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

public class TestModel
{
      public string Blarg { get; set; }
}