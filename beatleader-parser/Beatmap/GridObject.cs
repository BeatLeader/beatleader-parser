using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GridObject : BeatmapObject
    {
        [JsonProperty(PropertyName = "x")]
        public int X { get; set; }
        [JsonProperty(PropertyName = "y")]
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GridObject @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   X == @object.X &&
                   Y == @object.Y;
        }

        public override int GetHashCode()
        {
            int hashCode = 23480321;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
