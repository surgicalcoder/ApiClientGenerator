using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
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
}