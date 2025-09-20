using System.Text.Json.Serialization;

namespace BeatMapParser.Map.Difficulty.V3.Base
{
    public class BeatmapGridObject : BeatmapObject
    {
        [JsonPropertyName("x")]
        public int x { get; set; }
        [JsonPropertyName("y")]
        public int y { get; set; }
        [JsonIgnore]
        public float njs { get; set; }
        [JsonIgnore]
        public virtual GridObjectCustomData? customData { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BeatmapGridObject @object &&
                   base.Equals(obj) &&
                   Beats == @object.Beats &&
                   x == @object.x &&
                   y == @object.y;
        }

        public override int GetHashCode()
        {
            int hashCode = 72470987;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
    }
}
