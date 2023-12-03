using Newtonsoft.Json;

namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapObject
    {
        [JsonProperty(PropertyName = "b")]
        public float Beats { get; set; }
        [JsonIgnore]
        public float Seconds { get; set; } = 0f;
        [JsonIgnore]
        public float BpmTime { get; set; } = 0f;

        public override bool Equals(object obj) => obj is BeatmapObject other && Beats == other.Beats;

        public override int GetHashCode() => 650526578 + Beats.GetHashCode();
    }
}
