using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class Chain : SliderObject
    {
        [JsonProperty(PropertyName = "sc")]
        public int SegmentCount { get; set; }
        [JsonProperty(PropertyName = "s")]
        public float SquishFactor { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Chain @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   X == @object.X &&
                   Y == @object.Y &&
                   Color == @object.Color &&
                   Direction == @object.Direction &&
                   HeadMultiplier == @object.HeadMultiplier &&
                   TailBeat == @object.TailBeat &&
                   TailX == @object.TailX &&
                   TailY == @object.TailY &&
                   SegmentCount == @object.SegmentCount &&
                   SquishFactor == @object.SquishFactor;
        }

        public override int GetHashCode()
        {
            int hashCode = 1104026986;
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
            hashCode = hashCode * -1521134295 + SegmentCount.GetHashCode();
            hashCode = hashCode * -1521134295 + SquishFactor.GetHashCode();
            return hashCode;
        }
    }
}
