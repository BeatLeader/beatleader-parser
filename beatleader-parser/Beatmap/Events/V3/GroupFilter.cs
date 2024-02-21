using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupFilter
    {
        [JsonProperty(PropertyName = "c")]
        public int Chunks { get; set; }
        [JsonProperty(PropertyName = "f")]
        public GroupFilterType Type { get; set; }
        [JsonProperty(PropertyName = "p")]
        public int Parameter0 { get; set; }
        [JsonProperty(PropertyName = "t")]
        public int Parameter1 { get; set; }
        [JsonProperty(PropertyName = "r")]
        public bool Reverse { get; set; }
        [JsonProperty(PropertyName = "n")]
        public GroupRandomBehaviour RandomBehaviour { get; set; }
        [JsonProperty(PropertyName = "s")]
        public int RandomSeed { get; set; }
        [JsonProperty(PropertyName = "l")]
        public float LimitPercentage { get; set; }
        [JsonProperty(PropertyName = "d")]
        public GroupLimitBehaviour LimitBehaviour { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupFilter filter &&
                   Chunks == filter.Chunks &&
                   Type == filter.Type &&
                   Parameter0 == filter.Parameter0 &&
                   Parameter1 == filter.Parameter1 &&
                   Reverse == filter.Reverse &&
                   RandomBehaviour == filter.RandomBehaviour &&
                   RandomSeed == filter.RandomSeed &&
                   LimitPercentage == filter.LimitPercentage &&
                   LimitBehaviour == filter.LimitBehaviour;
        }

        public override int GetHashCode()
        {
            int hashCode = -542463849;
            hashCode = hashCode * -1521134295 + Chunks.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Parameter0.GetHashCode();
            hashCode = hashCode * -1521134295 + Parameter1.GetHashCode();
            hashCode = hashCode * -1521134295 + Reverse.GetHashCode();
            hashCode = hashCode * -1521134295 + RandomBehaviour.GetHashCode();
            hashCode = hashCode * -1521134295 + RandomSeed.GetHashCode();
            hashCode = hashCode * -1521134295 + LimitPercentage.GetHashCode();
            hashCode = hashCode * -1521134295 + LimitBehaviour.GetHashCode();
            return hashCode;
        }
    }

    public enum GroupFilterType
    {
        Division = 1,
        StepAndOffset = 2,
    }

    public enum GroupRandomBehaviour
    {
        NoRandom = 0,
        KeepOrder = 1,
        RandomElements = 2,
    }

    public enum GroupLimitBehaviour
    {
        None = 0,
        Duration = 1,
        Distribution = 2,
    }
}
