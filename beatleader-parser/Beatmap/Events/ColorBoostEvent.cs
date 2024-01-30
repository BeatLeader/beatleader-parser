using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class ColorBoostEvent : EventObject
    {
        [JsonProperty(PropertyName = "o")]
        public bool Boost { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ColorBoostEvent @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   Boost == @object.Boost;
        }

        public override int GetHashCode()
        {
            int hashCode = 1346906239;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + Boost.GetHashCode();
            return hashCode;
        }
    }
}
