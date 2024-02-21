using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class EventGroup : EventObject
    {
        [JsonProperty(PropertyName = "g")]
        public int Group { get; set; }
        [JsonProperty(PropertyName = "e")]
        public List<GroupLane> Lanes { get; set; }

        public override bool Equals(object obj)
        {
            return obj is EventGroup group &&
                   base.Equals(obj) &&
                   Beat == group.Beat &&
                   Group == group.Group &&
                   EqualityComparer<List<GroupLane>>.Default.Equals(Lanes, group.Lanes);
        }

        public override int GetHashCode()
        {
            int hashCode = -1775234517;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + Group.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<GroupLane>>.Default.GetHashCode(Lanes);
            return hashCode;
        }
    }
}
