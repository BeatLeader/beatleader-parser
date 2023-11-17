using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Wall : BeatmapGridObject
    {
        [JsonProperty(PropertyName = "d")]
        public float DurationInBeats { get; set; }
        [JsonIgnore]
        public float DurationInSeconds { get; set; } = 0;
        [JsonProperty(PropertyName = "w")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "h")]
        public int Height { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Wall otherWall = (Wall)obj;
            return Equals(Beats, otherWall.Beats) &&
                   Equals(DurationInBeats, otherWall.DurationInBeats) &&
                   Equals(x, otherWall.x) &&
                   Equals(y, otherWall.y) &&
                   Equals(Width, otherWall.Width) &&
                   Equals(Height, otherWall.Height);
        }
    }
}
