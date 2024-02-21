using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupLaneDistribution : GroupLane
    {
        [JsonProperty(PropertyName = "s")]
        public float Distribution { get; set; }
        [JsonProperty(PropertyName = "t")]
        public GroupDistributionType DistributionType { get; set; }
        [JsonProperty(PropertyName = "b")]
        public bool DistributionAffectsFirst { get; set; }
        [JsonProperty(PropertyName = "i")]
        public int DistributionEasing { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupLaneDistribution distribution &&
                   base.Equals(obj) &&
                   EqualityComparer<GroupFilter>.Default.Equals(GroupFilter, distribution.GroupFilter) &&
                   BeatDistribution == distribution.BeatDistribution &&
                   BeatDistributionType == distribution.BeatDistributionType &&
                   Distribution == distribution.Distribution &&
                   DistributionType == distribution.DistributionType &&
                   DistributionAffectsFirst == distribution.DistributionAffectsFirst &&
                   DistributionEasing == distribution.DistributionEasing;
        }

        public override int GetHashCode()
        {
            int hashCode = 1132525889;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<GroupFilter>.Default.GetHashCode(GroupFilter);
            hashCode = hashCode * -1521134295 + BeatDistribution.GetHashCode();
            hashCode = hashCode * -1521134295 + BeatDistributionType.GetHashCode();
            hashCode = hashCode * -1521134295 + Distribution.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionType.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionAffectsFirst.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionEasing.GetHashCode();
            return hashCode;
        }
    }
}
