using System;
using System.Collections.Generic;

namespace GoLive.Generator.ApiClientGenerator
{
    public class RouteGeneratorSettings
    {
        public String OutputFile { get; set; }
        public List<String> OutputFiles { get; set; }
        public List<String> Includes { get; set; }

        public string CustomDiscriminator { get; set; }
        public string Namespace { get; set; }

        public List<String> PreAppendLines { get; set; }
        public List<String> PostAppendLines { get; set; }
    }
}