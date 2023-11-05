using Parser.Map.Difficulty.V2.Grid;
using Parser.Map.Difficulty.V2.Event;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V2.Base
{
    public class DifficultyV2
    {
        public string _version { get; set; } = "";
        public List<Note> _notes { get; set; } = new();
        public List<Slider> _sliders { get; set; } = new();
        public List<Obstacle> _obstacles { get; set; } = new();
        public List<Events> _events { get; set; } = new();
    }
}
