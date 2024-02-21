using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupRotationEvent : GroupMovementEvent
    {
        [JsonProperty(PropertyName = "l")]
        public int Loops { get; set; }
        [JsonProperty(PropertyName = "r")]
        public float Rotation { get; set; }
        [JsonProperty(PropertyName = "o")]
        public RotationDirection RotationDirection { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupRotationEvent @event &&
                   base.Equals(obj) &&
                   Beat == @event.Beat &&
                   ExtendsRotation == @event.ExtendsRotation &&
                   EaseType == @event.EaseType &&
                   Loops == @event.Loops &&
                   Rotation == @event.Rotation &&
                   RotationDirection == @event.RotationDirection;
        }

        public override int GetHashCode()
        {
            int hashCode = 1148446669;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + ExtendsRotation.GetHashCode();
            hashCode = hashCode * -1521134295 + EaseType.GetHashCode();
            hashCode = hashCode * -1521134295 + Loops.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            hashCode = hashCode * -1521134295 + RotationDirection.GetHashCode();
            return hashCode;
        }
    }

    public enum RotationDirection
    {
        Automatic = 0,
        Clockwise = 1,
        CounterClockwise = 2
    }
}
