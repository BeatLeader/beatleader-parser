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
        public int Segment { get; set; }
        [JsonProperty(PropertyName = "s")]
        public float Squish { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Chain otherChain = (Chain)obj;
            return Equals(Beats, otherChain.Beats) &&
                   Equals(Color, otherChain.Color) &&
                   Equals(x, otherChain.x) &&
                   Equals(y, otherChain.y) &&
                   Equals(Direction, otherChain.Direction) &&
                   Equals(TailInBeats, otherChain.TailInBeats) &&
                   Equals(tx, otherChain.tx) &&
                   Equals(ty, otherChain.ty) &&
                   Equals(Segment, otherChain.Segment) &&
                   Equals(Squish, otherChain.Squish);
        }
    }
}
