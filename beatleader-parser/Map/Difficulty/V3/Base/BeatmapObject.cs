using Newtonsoft.Json;
using System;

namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapObject : IComparable<BeatmapObject>
    {
        [JsonProperty(PropertyName = "b")]
        public float Beats { get; set; }
        [JsonIgnore]
        public float Seconds { get; set; } = 0f;
        [JsonIgnore]
        public float BpmTime { get; set; } = 0f;
        public float BpmChangeStartTime { get; set; } = 0f;

        public override bool Equals(object obj) => obj is BeatmapObject other && Beats == other.Beats;

        public override int GetHashCode() => 650526578 + Beats.GetHashCode();
        int IComparable<BeatmapObject>.CompareTo(BeatmapObject other) => this.Beats.CompareTo(other.Beats);
    }
}
