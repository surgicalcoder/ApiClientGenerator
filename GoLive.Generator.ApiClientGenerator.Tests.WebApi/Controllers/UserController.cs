using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly List<string> users = new() {
        "Tom", "Frank", "Nelly", "Tobias"
    };

    [HttpGet(Name = "GetUsers")]
    public IEnumerable<string> Get() => users;

    [HttpGet]
    public string? GetUser(int Id) => Id >= 0 && Id < users.Count ? users[Id] : null;

    [HttpPost]
    public int GetUser([FromBody] string user) {
        int id = users.Count;
        users.Add(user);
        return id;
    }

    [HttpPost]
    public string GetUser2(string Id, string Id2, ComplexObjectExample example)
    {
        return "ok";
    }

    public virtual string OverrideTest(string Id)
    {
        return "test";
    }

    [NonAction]
    public virtual string IgnoreAction()
    {
        return "Ignore me";
    }
    

    public class ComplexObjectExample
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}