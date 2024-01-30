using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class Arc : SliderObject
    {
        [JsonProperty(PropertyName = "tc")]
        public ObjectDirection TailDirection { get; set; }
        [JsonProperty(PropertyName = "tmu")]
        public float TailMultiplier { get; set; }
        [JsonProperty(PropertyName = "m")]
        public MidAnchorMode MidAnchorMode { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Arc @object &&
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
                   TailDirection == @object.TailDirection &&
                   TailMultiplier == @object.TailMultiplier &&
                   MidAnchorMode == @object.MidAnchorMode;
        }

        public override int GetHashCode()
        {
            int hashCode = 1113538273;
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
            hashCode = hashCode * -1521134295 + TailDirection.GetHashCode();
            hashCode = hashCode * -1521134295 + TailMultiplier.GetHashCode();
            hashCode = hashCode * -1521134295 + MidAnchorMode.GetHashCode();
            return hashCode;
        }
    }

    public enum MidAnchorMode
    {
        Straight = 0,
        Clockwise = 1,
        CounterClockwise = 2,
    }
}
