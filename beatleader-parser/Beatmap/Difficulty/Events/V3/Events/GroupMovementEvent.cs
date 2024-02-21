using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupMovementEvent
    {
        [JsonProperty(PropertyName = "b")]
        public float Beat { get; set; }
        [JsonProperty(PropertyName = "p")]
        public bool ExtendsRotation { get; set; }
        [JsonProperty(PropertyName = "e")]
        public int EaseType { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupMovementEvent @event &&
                   Beat == @event.Beat &&
                   ExtendsRotation == @event.ExtendsRotation &&
                   EaseType == @event.EaseType;
        }

        public override int GetHashCode()
        {
            int hashCode = 1003541872;
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + ExtendsRotation.GetHashCode();
            hashCode = hashCode * -1521134295 + EaseType.GetHashCode();
            return hashCode;
        }
    }
}
