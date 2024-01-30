using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class Obstacle : GridObject
    {
        [JsonProperty(PropertyName = "d")]
        public float Duration { get; set; }
        [JsonProperty(PropertyName = "w")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "h")]
        public int Height { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Obstacle @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   X == @object.X &&
                   Y == @object.Y &&
                   Duration == @object.Duration &&
                   Width == @object.Width &&
                   Height == @object.Height;
        }

        public override int GetHashCode()
        {
            int hashCode = -741246991;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Duration.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }
    }
}
