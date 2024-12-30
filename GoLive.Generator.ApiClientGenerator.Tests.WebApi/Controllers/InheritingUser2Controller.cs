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