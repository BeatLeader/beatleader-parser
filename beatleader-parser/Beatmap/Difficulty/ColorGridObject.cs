using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class ColorGridObject : GridObject
    {
        [JsonProperty(PropertyName = "c")]
        public GridObjectColor Color { get; set; }
        [JsonProperty(PropertyName = "d")]
        public ObjectDirection Direction { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ColorGridObject @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   X == @object.X &&
                   Y == @object.Y &&
                   Color == @object.Color &&
                   Direction == @object.Direction;
        }

        public override int GetHashCode()
        {
            int hashCode = 51125002;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            return hashCode;
        }
    }

    public enum GridObjectColor
    {
        Red = 0,
        Blue = 1,
    }

    public enum ObjectDirection
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        UpLeft = 4,
        UpRight = 5,
        DownLeft = 6,
        DownRight = 7,
        Any = 8,
    }
}
