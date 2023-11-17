using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;
using System;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Note : BeatmapColorGridObject
    {
        [JsonProperty(PropertyName = "a")]
        public int AngleOffset { get; set; }
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Note otherNote = (Note)obj;
            return Equals(Beats, otherNote.Beats) &&
                   Equals(Color, otherNote.Color) &&
                   Equals(x, otherNote.x) &&
                   Equals(y, otherNote.y) &&
                   Equals(AngleOffset, otherNote.AngleOffset) &&
                   Equals(CutDirection, otherNote.CutDirection);
        }
    }
}
