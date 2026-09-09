using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers;

public class InheritsCustomBaseController : CustomControllerBase
{
    [HttpGet]
    public IEnumerable<string> GetItems(string Filter)
    {
        return new[] { Filter };
    }

    [HttpDelete("{id}")]
    public string DeleteItem(string id)
    {
        return "deleted";
    }
}