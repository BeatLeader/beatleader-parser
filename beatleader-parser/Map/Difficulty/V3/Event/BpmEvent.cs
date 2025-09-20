using BeatMapParser.Map.Difficulty.V3.Base;
using System.Text.Json.Serialization;

namespace BeatMapParser.Map.Difficulty.V3.Event
{
    public class BpmEvent : BeatmapObject
    {
        [JsonPropertyName("m")]
        public float Bpm { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BpmEvent @event &&
                   base.Equals(obj) &&
                   Beats == @event.Beats &&
                   Bpm == @event.Bpm;
        }

        public override int GetHashCode()
        {
            int hashCode = 56753886;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + Bpm.GetHashCode();
            return hashCode;
        }
    }
}
