using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class BpmEvent : BeatmapObject
    {
        [JsonProperty(PropertyName = "m")]
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
