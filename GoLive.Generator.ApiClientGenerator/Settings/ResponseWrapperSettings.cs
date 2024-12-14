using System.Collections.Generic;

namespace GoLive.Generator.ApiClientGenerator.Settings;

public class ResponseWrapperSettings
{
    public bool Enabled { get; set; }
    public Dictionary<string, string> ExtractHeaders { get; set; } = new();
}