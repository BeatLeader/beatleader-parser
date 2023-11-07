using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Obstacle : BeatmapGridObject
    {
        [JsonProperty(PropertyName = "d")]
        public float DurationInBeats { get; set; }
        [JsonIgnore]
        public float DurationInSeconds { get; set; } = 0;
        [JsonProperty(PropertyName = "w")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "h")]
        public int Height { get; set; }
    }
}
