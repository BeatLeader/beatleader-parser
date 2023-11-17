using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class Colorboostbeatmapevent : BeatmapObject
    {
        [JsonProperty(PropertyName = "o")]
        public bool On { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Colorboostbeatmapevent otherEvent = (Colorboostbeatmapevent)obj;
            return Equals(Beats, otherEvent.Beats) &&
                   Equals(On, otherEvent.On);
        }
    }
}
