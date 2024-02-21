using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupFxEventLane : GroupLaneDistribution
    {
        [JsonProperty(PropertyName = "l")]
        public List<int> VfxBaseDataList { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupFxEventLane lane &&
                   base.Equals(obj) &&
                   EqualityComparer<GroupFilter>.Default.Equals(GroupFilter, lane.GroupFilter) &&
                   BeatDistribution == lane.BeatDistribution &&
                   BeatDistributionType == lane.BeatDistributionType &&
                   Distribution == lane.Distribution &&
                   DistributionType == lane.DistributionType &&
                   DistributionAffectsFirst == lane.DistributionAffectsFirst &&
                   DistributionEasing == lane.DistributionEasing &&
                   EqualityComparer<List<int>>.Default.Equals(VfxBaseDataList, lane.VfxBaseDataList);
        }

        public override int GetHashCode()
        {
            int hashCode = 1419395543;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<GroupFilter>.Default.GetHashCode(GroupFilter);
            hashCode = hashCode * -1521134295 + BeatDistribution.GetHashCode();
            hashCode = hashCode * -1521134295 + BeatDistributionType.GetHashCode();
            hashCode = hashCode * -1521134295 + Distribution.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionType.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionAffectsFirst.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionEasing.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(VfxBaseDataList);
            return hashCode;
        }
    }
}
