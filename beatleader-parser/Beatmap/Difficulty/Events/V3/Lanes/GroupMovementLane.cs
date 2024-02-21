using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap.Events.V3
{
    public class GroupMovementLane<T> : GroupLaneDistribution where T : GroupMovementEvent
    {
        [JsonProperty(PropertyName = "a")]
        public GroupAxis Axis { get; set; }
        [JsonProperty(PropertyName = "r")]
        public bool ReverseRotation { get; set; }
        [JsonProperty(PropertyName = "l")]
        public List<T> Events { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupMovementLane<T> lane &&
                   base.Equals(obj) &&
                   EqualityComparer<GroupFilter>.Default.Equals(GroupFilter, lane.GroupFilter) &&
                   BeatDistribution == lane.BeatDistribution &&
                   BeatDistributionType == lane.BeatDistributionType &&
                   Distribution == lane.Distribution &&
                   DistributionType == lane.DistributionType &&
                   DistributionAffectsFirst == lane.DistributionAffectsFirst &&
                   DistributionEasing == lane.DistributionEasing &&
                   Axis == lane.Axis &&
                   ReverseRotation == lane.ReverseRotation &&
                   EqualityComparer<List<T>>.Default.Equals(Events, lane.Events);
        }

        public override int GetHashCode()
        {
            int hashCode = -1397631384;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<GroupFilter>.Default.GetHashCode(GroupFilter);
            hashCode = hashCode * -1521134295 + BeatDistribution.GetHashCode();
            hashCode = hashCode * -1521134295 + BeatDistributionType.GetHashCode();
            hashCode = hashCode * -1521134295 + Distribution.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionType.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionAffectsFirst.GetHashCode();
            hashCode = hashCode * -1521134295 + DistributionEasing.GetHashCode();
            hashCode = hashCode * -1521134295 + Axis.GetHashCode();
            hashCode = hashCode * -1521134295 + ReverseRotation.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<T>>.Default.GetHashCode(Events);
            return hashCode;
        }
    }

    public enum GroupAxis
    {
        X = 0,
        Y = 1,
        Z = 2,
    }
}
