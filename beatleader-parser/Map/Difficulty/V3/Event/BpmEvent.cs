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
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            BpmEvent otherBpmEvent = (BpmEvent)obj;
            return Equals(Beats, otherBpmEvent.Beats) &&
                   Equals(Bpm, otherBpmEvent.Bpm);
        }
    }
}
