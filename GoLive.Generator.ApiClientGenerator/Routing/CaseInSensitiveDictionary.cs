using System;
using System.Collections.Generic;

namespace GoLive.Generator.ApiClientGenerator.Routing;

public class CaseInSensitiveDictionary : Dictionary<string, string>
{
    public CaseInSensitiveDictionary() : base(StringComparer.InvariantCultureIgnoreCase){}
}