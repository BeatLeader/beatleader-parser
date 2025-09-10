using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapColorGridObjectWithTail : BeatmapColorGridObject
    {
        [JsonPropertyName("d")]
        public int CutDirection { get; set; }
        [JsonPropertyName("tb")]
        public float TailInBeats { get; set; }
        [JsonPropertyName("tc")]
        public int TailCutDirection { get; set; }
        [JsonIgnore]
        public float TailInSeconds { get; set; } = 0f;
        [JsonIgnore]
        public float TailBpmTime { get; set; } = 0f;
        public int tx { get; set; }
        public int ty { get; set; }
    }
}
