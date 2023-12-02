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
            return obj is Colorboostbeatmapevent colorboostbeatmapevent &&
                   base.Equals(obj) &&
                   Beats == colorboostbeatmapevent.Beats &&
                   On == colorboostbeatmapevent.On;
        }

        public override int GetHashCode()
        {
            int hashCode = -1587591832;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + On.GetHashCode();
            return hashCode;
        }
    }
}
