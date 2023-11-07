using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Chain : BeatmapGridObject
    {
        [JsonProperty(PropertyName = "c")]
        public int Color { get; set; }
        [JsonProperty(PropertyName = "d")]
        public int Direction { get; set; }
        [JsonProperty(PropertyName = "tb")]
        public float TailInBeats { get; set; }
        [JsonIgnore]
        public float TailInSeconds { get; set; } = 0f;
        [JsonIgnore]
        public float TailBpmTime { get; set; } = 0f;
        public int tx { get; set; }
        public int ty { get; set; }
        [JsonProperty(PropertyName = "sc")]
        public int Segment { get; set; }
        [JsonProperty(PropertyName = "s")]
        public float Squish { get; set; }
    }
}
