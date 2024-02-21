using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class FxEventGroup : EventGroup<GroupFxEventLane>
    {
        [JsonProperty(PropertyName = "t")]
        public FxEventType Type { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FxEventGroup group &&
                   base.Equals(obj) &&
                   Beat == group.Beat &&
                   GroupId == group.GroupId &&
                   EqualityComparer<List<GroupFxEventLane>>.Default.Equals(Lanes, group.Lanes) &&
                   Type == group.Type;
        }

        public override int GetHashCode()
        {
            int hashCode = -213830155;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + GroupId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<GroupFxEventLane>>.Default.GetHashCode(Lanes);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }
    }

    public enum FxEventType
    {
        Int,
        Float,
        Bool,
    }
}
