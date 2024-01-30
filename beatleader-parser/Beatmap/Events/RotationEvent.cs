using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class RotationEvent : EventObject
    {
        [JsonProperty(PropertyName = "e")]
        public RotationEventType Type { get; set; }
        [JsonProperty(PropertyName = "r")]
        public float Rotation { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RotationEvent @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   Type == @object.Type &&
                   Rotation == @object.Rotation;
        }

        public override int GetHashCode()
        {
            int hashCode = -1477558625;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            return hashCode;
        }
    }

    public enum RotationEventType
    {
        EarlyRotation = 0,
        LateRotation = 1,
    }
}
