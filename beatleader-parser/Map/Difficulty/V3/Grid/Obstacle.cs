using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Obstacle : BeatmapGridObject
    {
        public float d { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }
}
