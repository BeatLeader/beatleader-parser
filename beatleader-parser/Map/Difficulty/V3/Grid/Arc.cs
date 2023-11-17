using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Arc : BeatmapGridObject
    {
        [JsonProperty(PropertyName = "c")]
        public int Color { get; set; }
        [JsonProperty(PropertyName = "d")]
        public int CutDirection { get; set; }
        [JsonProperty(PropertyName = "mu")]
        public float Multiplier { get; set; }
        [JsonProperty(PropertyName = "tb")]
        public float TailInBeats { get; set; }
        [JsonIgnore]
        public float TailInSeconds { get; set; } = 0f;
        [JsonIgnore]
        public float TailBpmTime { get; set; } = 0f;
        public int tx { get; set; }
        public int ty { get; set; }
        [JsonProperty(PropertyName = "tc")]
        public int TailDirection { get; set; }
        [JsonProperty(PropertyName = "tmu")]
        public float TailMultiplier { get; set; }
        [JsonProperty(PropertyName = "m")]
        public int AnchorMode { get; set; }

        public enum MidAnchorMode
        {
            Straight = 0,
            Clockwise = 1,
            CounterClockwise = 2
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Arc otherChain = (Arc)obj;
            return Equals(Beats, otherChain.Beats) &&
                   Equals(Color, otherChain.Color) &&
                   Equals(x, otherChain.x) &&
                   Equals(y, otherChain.y) &&
                   Equals(CutDirection, otherChain.CutDirection) &&
                   Equals(TailInBeats, otherChain.TailInBeats) &&
                   Equals(tx, otherChain.tx) &&
                   Equals(ty, otherChain.ty) &&
                   Equals(TailDirection, otherChain.TailDirection) &&
                   Equals(TailMultiplier, otherChain.TailMultiplier) &&
                   Equals(AnchorMode, otherChain.AnchorMode);
        }
    }
}
