using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

[Area("AreaAreaTest")]
public class AreaTestController : ControllerBase
{
    [HttpGet]
    public CancellationTokenTestController.InfoData GetInfo(int Id, CancellationToken ct)
    {
        /*ApiClient client;
        client.AreaAreaTest.AreaTestClient.*/
        return new CancellationTokenTestController.InfoData(Id);
    }
}