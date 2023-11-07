using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class Colorboostbeatmapevent : BeatmapObject
    {
        [JsonProperty(PropertyName = "o")]
        public bool On { get; set; }
    }
}
