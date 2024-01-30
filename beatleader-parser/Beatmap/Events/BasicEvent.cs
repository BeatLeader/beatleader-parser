using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class BasicEvent : EventObject
    {
        [JsonProperty(PropertyName = "et")]
        public EventType EventType { get; set; }
        [JsonProperty(PropertyName = "i")]
        public int Value { get; set; }
        [JsonProperty(PropertyName = "f")]
        public float FloatValue { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BasicEvent @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   EventType == @object.EventType &&
                   Value == @object.Value &&
                   FloatValue == @object.FloatValue;
        }

        public override int GetHashCode()
        {
            int hashCode = 1082676020;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + EventType.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + FloatValue.GetHashCode();
            return hashCode;
        }
    }

    public enum EventType
    {
        BackLasers = 0,
        RingLights = 1,
        LeftLasers = 2,
        RightLasers = 3,
        CenterLights = 4,
        RingSpin = 8,
        RingZoom = 9,
        ExtraLights = 10,
        LeftLasersSpeed = 12,
        RightLasersSpeed = 13,
    }
}
