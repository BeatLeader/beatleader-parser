using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class EventGroup<T> : EventObject where T : GroupLane
    {
        [JsonProperty(PropertyName = "g")]
        public int GroupId { get; set; }
        [JsonProperty(PropertyName = "e")]
        public List<T> Lanes { get; set; }

        public override bool Equals(object obj)
        {
            return obj is EventGroup<T> group &&
                   base.Equals(obj) &&
                   Beat == group.Beat &&
                   GroupId == group.GroupId &&
                   EqualityComparer<List<T>>.Default.Equals(Lanes, group.Lanes);
        }

        public override int GetHashCode()
        {
            int hashCode = 2134647368;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + GroupId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<T>>.Default.GetHashCode(Lanes);
            return hashCode;
        }
    }
}
