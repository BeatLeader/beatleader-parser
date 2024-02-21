using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupLane
    {
        [JsonProperty(PropertyName = "f")]
        public GroupFilter GroupFilter { get; set; }
        [JsonProperty(PropertyName = "w")]
        public float BeatDistribution { get; set; }
        [JsonProperty(PropertyName = "d")]
        public GroupDistributionType BeatDistributionType { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupLane lane &&
                   EqualityComparer<GroupFilter>.Default.Equals(GroupFilter, lane.GroupFilter) &&
                   BeatDistribution == lane.BeatDistribution &&
                   BeatDistributionType == lane.BeatDistributionType;
        }

        public override int GetHashCode()
        {
            int hashCode = -1832246204;
            hashCode = hashCode * -1521134295 + EqualityComparer<GroupFilter>.Default.GetHashCode(GroupFilter);
            hashCode = hashCode * -1521134295 + BeatDistribution.GetHashCode();
            hashCode = hashCode * -1521134295 + BeatDistributionType.GetHashCode();
            return hashCode;
        }
    }

    public enum GroupDistributionType
    {
        Wave = 1,
        Step = 2,
    }
}
