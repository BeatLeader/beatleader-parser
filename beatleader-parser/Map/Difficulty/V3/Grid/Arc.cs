using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Arc : BeatmapColorGridObjectWithTail
    {
        [JsonProperty(PropertyName = "mu")]
        public float Multiplier { get; set; }
        [JsonProperty(PropertyName = "tc")]
        public int TailDirection { get; set; }
        [JsonProperty(PropertyName = "tmu")]
        public float TailMultiplier { get; set; }
        [JsonProperty(PropertyName = "m")]
        public int AnchorMode { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Arc arc &&
                   base.Equals(obj) &&
                   Beats == arc.Beats &&
                   x == arc.x &&
                   y == arc.y &&
                   Color == arc.Color &&
                   CutDirection == arc.CutDirection &&
                   TailInBeats == arc.TailInBeats &&
                   tx == arc.tx &&
                   ty == arc.ty &&
                   TailDirection == arc.TailDirection &&
                   TailMultiplier == arc.TailMultiplier &&
                   AnchorMode == arc.AnchorMode;
        }

        public override int GetHashCode()
        {
            int hashCode = 1500751558;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + CutDirection.GetHashCode();
            hashCode = hashCode * -1521134295 + TailInBeats.GetHashCode();
            hashCode = hashCode * -1521134295 + tx.GetHashCode();
            hashCode = hashCode * -1521134295 + ty.GetHashCode();
            hashCode = hashCode * -1521134295 + TailDirection.GetHashCode();
            hashCode = hashCode * -1521134295 + TailMultiplier.GetHashCode();
            hashCode = hashCode * -1521134295 + AnchorMode.GetHashCode();
            return hashCode;
        }

        public enum MidAnchorMode
        {
            Straight = 0,
            Clockwise = 1,
            CounterClockwise = 2
        }
    }
}
