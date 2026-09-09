using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

public class CancellationTokenTestController : ControllerBase
{
    [HttpGet]
    public InfoData GetInfo(int Id, CancellationToken ct)
    {
        return new InfoData(Id);
    }

    [HttpDelete]
    public InfoData Delete(string Id, string zoneId, CancellationToken ct)
    {
        return new InfoData(Id.Length + zoneId.Length);
    }

    public record InfoData(int Id);
}