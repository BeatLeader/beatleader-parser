using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class BpmEvent : BeatmapObject
    {
        [JsonProperty(PropertyName = "m")]
        public float Bpm { get; set; }
    }
}
