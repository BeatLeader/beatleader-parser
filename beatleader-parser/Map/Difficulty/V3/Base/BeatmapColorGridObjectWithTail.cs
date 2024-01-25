using Newtonsoft.Json;

namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapColorGridObjectWithTail : BeatmapColorGridObject
    {
        [JsonProperty(PropertyName = "d")]
        public int CutDirection { get; set; }
        [JsonProperty(PropertyName = "tb")]
        public float TailInBeats { get; set; }
        [JsonIgnore]
        public float TailInSeconds { get; set; } = 0f;
        [JsonIgnore]
        public float TailBpmTime { get; set; } = 0f;
        public int tx { get; set; }
        public int ty { get; set; }
    }
}
