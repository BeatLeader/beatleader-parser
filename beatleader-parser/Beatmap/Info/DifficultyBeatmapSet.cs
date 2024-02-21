using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class DifficultyBeatmapSet
    {
        [JsonProperty(PropertyName = "_beatmapCharacteristicName")]
        public BeatmapCharacteristicName BeatmapCharacteristicName { get; set; }
        [JsonProperty(PropertyName = "_difficultyBeatmaps")]
        public List<DifficultyBeatmap> DifficultyBeatmaps { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DifficultyBeatmapSet set &&
                   BeatmapCharacteristicName == set.BeatmapCharacteristicName &&
                   EqualityComparer<List<DifficultyBeatmap>>.Default.Equals(DifficultyBeatmaps, set.DifficultyBeatmaps);
        }

        public override int GetHashCode()
        {
            int hashCode = 1636646406;
            hashCode = hashCode * -1521134295 + BeatmapCharacteristicName.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<DifficultyBeatmap>>.Default.GetHashCode(DifficultyBeatmaps);
            return hashCode;
        }
    }

    public enum BeatmapCharacteristicName
    {
        Standard,
        NoArrows,
        OneSaber,
        _360Degree,
        _90Degree,
        Legacy,
        Lightshow,
        Lawless
    }
}
