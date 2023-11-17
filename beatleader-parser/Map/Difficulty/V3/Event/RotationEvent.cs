using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class RotationEvent : BeatmapObject
    {
        [JsonProperty(PropertyName = "e")]
        public int Event { get; set; }
        [JsonProperty(PropertyName = "r")]
        public int Rotation { get; set; }

        public enum EventType
        {
            Early = 0,
            Late = 1
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            RotationEvent otherEvent = (RotationEvent)obj;
            return Equals(Beats, otherEvent.Beats) &&
                   Equals(Event, otherEvent.Event) &&
                   Equals(Rotation, otherEvent.Rotation);
        }
    }
}
