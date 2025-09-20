using BeatMapParser.Map.Difficulty.V3.Base;

namespace BeatMapParser.Map.Difficulty.V3.Custom
{
    public class Bookmark : BeatmapObject
    {
        public string n { get; set; }
        public float[] c { get; set; }
    }
}
