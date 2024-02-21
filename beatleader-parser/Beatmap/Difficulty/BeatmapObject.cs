using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class BeatmapObject
    {
        [JsonProperty(PropertyName = "b")]
        public float Beat { get; set; }

        [JsonIgnore]
        public float Second { get; set; }
        [JsonIgnore]
        public float AbsoluteBeat { get; set; }

        public override bool Equals(object obj) => obj is BeatmapObject other && Beat == other.Beat;

        public override int GetHashCode() => 650526578 + Beat.GetHashCode();
    }
}
