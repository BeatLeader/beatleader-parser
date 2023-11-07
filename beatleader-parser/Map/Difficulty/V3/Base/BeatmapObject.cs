using Newtonsoft.Json;

namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapObject
    {
        [JsonProperty(PropertyName = "b")]
        public float Beats { get; set; }
        [JsonIgnore]
        public float Seconds { get; set; }
    }
}
