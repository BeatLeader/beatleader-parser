using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class RotationEvent : BeatmapObject
    {
        [JsonProperty(PropertyName = "e")]
        public int Event { get; set; }
        [JsonProperty(PropertyName = "r")]
        public float Rotation { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RotationEvent @event &&
                   base.Equals(obj) &&
                   Beats == @event.Beats &&
                   Event == @event.Event &&
                   Rotation == @event.Rotation;
        }

        public override int GetHashCode()
        {
            int hashCode = -1519484466;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + Event.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            return hashCode;
        }

        public enum EventType
        {
            Early = 0,
            Late = 1
        }
    }
}
