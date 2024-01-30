using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class Note : ColorGridObject
    {
        [JsonProperty(PropertyName = "a")]
        public int AngleOffset { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Note @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   X == @object.X &&
                   Y == @object.Y &&
                   Color == @object.Color &&
                   Direction == @object.Direction &&
                   AngleOffset == @object.AngleOffset;
        }

        public override int GetHashCode()
        {
            int hashCode = 1785099512;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + AngleOffset.GetHashCode();
            return hashCode;
        }
    }
}
