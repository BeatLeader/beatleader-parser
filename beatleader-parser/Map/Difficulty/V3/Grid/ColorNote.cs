using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Colornote : BeatmapGridObject
    {
        [JsonProperty(PropertyName = "a")]
        public int AngleOffset { get; set; }
        [JsonProperty(PropertyName = "c")]
        public int Color { get; set; }
        [JsonProperty(PropertyName = "d")]
        public int CutDirection { get; set; }

        public enum Type
        {
            Red = 0,
            Blue = 1
        }

        public enum Direction
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3,
            UpLeft = 4,
            UpRight = 5,
            DownLeft = 6,
            DownRight = 7,
            Any = 8
        }
    }
}
