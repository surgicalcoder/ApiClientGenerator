using System.Collections.Generic;
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
    
    
    
}

public class JSONDynamicTest
{
    public string Param1 { get; set; }
    public Dictionary<string, object> Object2 { get; set; }
}