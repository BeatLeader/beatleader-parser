using Parser.Map.Difficulty.V3.Base;
using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Note : BeatmapColorGridObject
    {
        [JsonPropertyName("a")]
        public int AngleOffset { get; set; }
        [JsonPropertyName("d")]
        public int CutDirection { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Note note &&
                   base.Equals(obj) &&
                   Beats == note.Beats &&
                   x == note.x &&
                   y == note.y &&
                   Color == note.Color &&
                   AngleOffset == note.AngleOffset &&
                   CutDirection == note.CutDirection;
        }

        public override int GetHashCode()
        {
            int hashCode = -2068007110;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + AngleOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + CutDirection.GetHashCode();
            return hashCode;
        }

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
