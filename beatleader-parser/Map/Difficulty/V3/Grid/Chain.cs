using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Chain : BeatmapColorGridObject
    {
        [JsonProperty(PropertyName = "d")]
        public int Direction { get; set; }
        [JsonProperty(PropertyName = "tb")]
        public float TailInBeats { get; set; }
        [JsonIgnore]
        public float TailInSeconds { get; set; } = 0f;
        [JsonIgnore]
        public float TailBpmTime { get; set; } = 0f;
        public int tx { get; set; }
        public int ty { get; set; }
        [JsonProperty(PropertyName = "sc")]
        public int Segment { get; set; } = 8;
        [JsonProperty(PropertyName = "s")]
        public float Squish { get; set; } = 1f;

        public override bool Equals(object obj)
        {
            return obj is Chain chain &&
                   base.Equals(obj) &&
                   Beats == chain.Beats &&
                   x == chain.x &&
                   y == chain.y &&
                   Color == chain.Color &&
                   Direction == chain.Direction &&
                   TailInBeats == chain.TailInBeats &&
                   tx == chain.tx &&
                   ty == chain.ty &&
                   Segment == chain.Segment &&
                   Squish == chain.Squish;
        }

        public override int GetHashCode()
        {
            int hashCode = 615981555;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + TailInBeats.GetHashCode();
            hashCode = hashCode * -1521134295 + tx.GetHashCode();
            hashCode = hashCode * -1521134295 + ty.GetHashCode();
            hashCode = hashCode * -1521134295 + Segment.GetHashCode();
            hashCode = hashCode * -1521134295 + Squish.GetHashCode();
            return hashCode;
        }
    }
}
