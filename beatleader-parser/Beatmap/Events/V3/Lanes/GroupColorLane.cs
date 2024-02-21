using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupColorLane : GroupLane
    {
        [JsonProperty(PropertyName = "r")]
        public float Distribution { get; set; }
        [JsonProperty(PropertyName = "t")]
        public GroupDistributionType DistributionType { get; set; }
        [JsonProperty(PropertyName = "b")]
        public bool DistributionAffectsFirst { get; set; }
        [JsonProperty(PropertyName = "i")]
        public int DistributionEasing { get; set; }

        public List<GroupColorEvent> Events { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupColorLane lane &&
                   base.Equals(obj) &&
                   EqualityComparer<GroupFilter>.Default.Equals(GroupFilter, lane.GroupFilter) &&
                   BeatDistribution == lane.BeatDistribution &&
                   BeatDistributionType == lane.BeatDistributionType &&
                   Distribution == lane.Distribution &&
                   DistributionType == lane.DistributionType &&
                   DistributionAffectsFirst == lane.DistributionAffectsFirst &&
                   DistributionEasing == lane.DistributionEasing &&
                   EqualityComparer<List<GroupColorEvent>>.Default.Equals(Events, lane.Events);
        }

        public override int GetHashCode()
        {
            int hashCode = 1244897549;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<GroupFilter>.Default.GetHashCode(GroupFilter);
            hashCode = hashCode * -1521134295 + BeatDistribution.GetHashCode();
            hashCode = hashCode * -1521134295 + BeatDistributionType.GetHashCode();
            hashCode = hashCode * -1521134295 + Distribution.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionType.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionAffectsFirst.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionEasing.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<GroupColorEvent>>.Default.GetHashCode(Events);
            return hashCode;
        }
    }
}
