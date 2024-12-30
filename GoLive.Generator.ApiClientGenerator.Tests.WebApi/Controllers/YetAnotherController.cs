using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

public class YetAnotherController : Controller
{
    [HttpPost]
    public string YetAnotherTest(string Id)
    {
        return "ok";
    }

    [HttpGet]
    public JSONDynamicTest JSONDynamicTestDynamic()
    {
        return new JSONDynamicTest();
    }

    [HttpOptions]
    public string HttpOptionsTest()
    {
        return "Ok";
    }
    
    
    [HttpHead]
    public string HttpHeadTest()
    {
        return "Ok";
    }

    
    [HttpPatch]
    public string HttpPatchTest()
    {
        return "Ok";
    }
}