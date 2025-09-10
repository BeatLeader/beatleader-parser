using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapColorGridObject : BeatmapGridObject
    {
        [JsonPropertyName("c")]
        public int Color { get; set; }
    }
}
