using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupColorEvent
    {
        [JsonProperty(PropertyName = "b")]
        public float Beat { get; set; }
        [JsonProperty(PropertyName = "i")]
        public int TransitionType { get; set; }
        [JsonProperty(PropertyName = "c")]
        public EventColor Color { get; set; }
        [JsonProperty(PropertyName = "s")]
        public float Brightness { get; set; }
        [JsonProperty(PropertyName = "f")]
        public int FlickerFrequency { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupColorEvent @event &&
                   Beat == @event.Beat &&
                   TransitionType == @event.TransitionType &&
                   Color == @event.Color &&
                   Brightness == @event.Brightness &&
                   FlickerFrequency == @event.FlickerFrequency;
        }

        public override int GetHashCode()
        {
            int hashCode = -591865552;
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + TransitionType.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + Brightness.GetHashCode();
            hashCode = hashCode * -1521134295 + FlickerFrequency.GetHashCode();
            return hashCode;
        }
    }

    public enum EventColor
    {
        Red = 0,
        Blue = 1,
        White = 2,
    }
}
