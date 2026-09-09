using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

public class InheritsCustomBaseController : CustomControllerBase
{
    [HttpPatch("{id}")]
    public ActionResult<CloudflareDnsRecordSummary> Update(string id, [FromQuery] string zoneId, [FromBody] CloudflareDnsRecordInput body, CancellationToken ct = default)
    {
        return default;
    }

    [HttpGet]
    public IEnumerable<string> GetItems(string Filter)
    {
        return new[] { Filter };
    }
}

public record CloudflareDnsRecordInput(string Name, string Content, int Ttl, bool Proxied);

public record CloudflareDnsRecordSummary(string Id, string Name);