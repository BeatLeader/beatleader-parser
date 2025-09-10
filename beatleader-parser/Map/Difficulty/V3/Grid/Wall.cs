using Parser.Map.Difficulty.V3.Base;
using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Wall : BeatmapGridObject
    {
        [JsonPropertyName("d")]
        public float DurationInBeats { get; set; }
        [JsonIgnore]
        public float DurationInSeconds { get; set; } = 0;
        [JsonPropertyName("w")]
        public int Width { get; set; }
        [JsonPropertyName("h")]
        public int Height { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Wall wall &&
                   base.Equals(obj) &&
                   Beats == wall.Beats &&
                   x == wall.x &&
                   y == wall.y &&
                   DurationInBeats == wall.DurationInBeats &&
                   Width == wall.Width &&
                   Height == wall.Height;
        }

        public override int GetHashCode()
        {
            int hashCode = 1947689683;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + DurationInBeats.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }
    }
}
