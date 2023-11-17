using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapColorGridObject : BeatmapGridObject
    {
        [JsonProperty(PropertyName = "c")]
        public int Color { get; set; }
    }
}
