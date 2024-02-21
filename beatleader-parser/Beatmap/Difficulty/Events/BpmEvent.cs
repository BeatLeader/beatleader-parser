using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class BpmEvent : EventObject
    {
        [JsonProperty(PropertyName = "m")]
        public float NewBpm { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BpmEvent @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   NewBpm == @object.NewBpm;
        }

        public override int GetHashCode()
        {
            int hashCode = -950173790;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + NewBpm.GetHashCode();
            return hashCode;
        }
    }
}
