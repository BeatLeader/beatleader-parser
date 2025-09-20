using BeatMapParser.Map.Difficulty.V3.Base;
using System.Text.Json.Serialization;

namespace BeatMapParser.Map.Difficulty.V3.Grid
{
    public class Chain : BeatmapColorGridObjectWithTail
    {
        [JsonPropertyName("sc")]
        public int SliceCount { get; set; } = 8;
        [JsonPropertyName("s")]
        public float Squish { get; set; } = 1f;

        public override bool Equals(object obj)
        {
            return obj is Chain chain &&
                   base.Equals(obj) &&
                   Beats == chain.Beats &&
                   x == chain.x &&
                   y == chain.y &&
                   Color == chain.Color &&
                   CutDirection == chain.CutDirection &&
                   TailInBeats == chain.TailInBeats &&
                   tx == chain.tx &&
                   ty == chain.ty &&
                   SliceCount == chain.SliceCount &&
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
            hashCode = hashCode * -1521134295 + CutDirection.GetHashCode();
            hashCode = hashCode * -1521134295 + TailInBeats.GetHashCode();
            hashCode = hashCode * -1521134295 + tx.GetHashCode();
            hashCode = hashCode * -1521134295 + ty.GetHashCode();
            hashCode = hashCode * -1521134295 + SliceCount.GetHashCode();
            hashCode = hashCode * -1521134295 + Squish.GetHashCode();
            return hashCode;
        }
    }
}
