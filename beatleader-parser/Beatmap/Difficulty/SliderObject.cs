using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class SliderObject : ColorGridObject
    {
        [JsonProperty(PropertyName = "mu")]
        public float HeadMultiplier { get; set; }
        [JsonProperty(PropertyName = "tb")]
        public float TailBeat { get; set; }
        [JsonProperty(PropertyName = "tx")]
        public int TailX { get; set; }
        [JsonProperty(PropertyName = "ty")]
        public int TailY { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SliderObject @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   X == @object.X &&
                   Y == @object.Y &&
                   Color == @object.Color &&
                   Direction == @object.Direction &&
                   HeadMultiplier == @object.HeadMultiplier &&
                   TailBeat == @object.TailBeat &&
                   TailX == @object.TailX &&
                   TailY == @object.TailY;
        }

        public override int GetHashCode()
        {
            int hashCode = -1126173413;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + HeadMultiplier.GetHashCode();
            hashCode = hashCode * -1521134295 + TailBeat.GetHashCode();
            hashCode = hashCode * -1521134295 + TailX.GetHashCode();
            hashCode = hashCode * -1521134295 + TailY.GetHashCode();
            return hashCode;
        }
    }
}
