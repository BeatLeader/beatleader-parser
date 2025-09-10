using Parser.Map.Difficulty.V3.Base;
using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V3.Event {
    public class NjsEvent : BeatmapObject {
        [JsonPropertyName("d")]
        public float Delta { get; set; }
        [JsonPropertyName("p")]
        public int UsePrevious { get; set; }
        [JsonPropertyName("e")]
        public int Easing { get; set; }

        public override bool Equals(object obj)
        {
            return obj is NjsEvent @event &&
                   base.Equals(obj) &&
                   Beats == @event.Beats &&
                   Delta == @event.Delta &&
                   UsePrevious == @event.UsePrevious &&
                   Easing == @event.Easing;
        }

        public override int GetHashCode()
        {
            int hashCode = 56753886;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + Delta.GetHashCode();
            hashCode = hashCode * -1521134295 + UsePrevious.GetHashCode();
            hashCode = hashCode * -1521134295 + Easing.GetHashCode();
            return hashCode;
        }
    }
}
