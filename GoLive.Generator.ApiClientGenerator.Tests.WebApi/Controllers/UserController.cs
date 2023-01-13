using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController
{
    private static readonly string[] Users = {
        "Tom", "Frank", "Nelly", "Tobias"
    };

    [HttpGet(Name = "GetUsers")]
    public IEnumerable<string> Get() => Users;

    [HttpGet]
    public string? GetUser(int Id) => Id >= 0 && Id < Users.Length ? Users[Id] : null;
}