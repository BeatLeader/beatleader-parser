using BeatMapParser.Map.Difficulty.V2.Event;
using BeatMapParser.Map.Difficulty.V2.Grid;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BeatMapParser.Map.Difficulty.V2.Base
{
    public class DifficultyV2
    {
        [JsonPropertyName("_version")]
        public string _version { get; set; } = "";
        [JsonPropertyName("_notes")]
        public List<Note> _notes { get; set; } = new();
        [JsonPropertyName("_sliders")]
        public List<Slider> _sliders { get; set; } = new();
        [JsonPropertyName("_obstacles")]
        public List<Obstacle> _obstacles { get; set; } = new();
        [JsonPropertyName("_events")]
        public List<Events> _events { get; set; } = new();
    }
}
