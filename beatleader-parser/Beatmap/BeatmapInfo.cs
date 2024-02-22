using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class BeatmapInfo
    {
        [JsonProperty(PropertyName = "_version")]
        public string Version { get; set; }
        [JsonProperty(PropertyName = "_songName")]
        public string SongName { get; set; }
        [JsonProperty(PropertyName = "_songSubName")]
        public string SongSubName { get; set; }
        [JsonProperty(PropertyName = "_songAuthorName")]
        public string SongAuthorName { get; set; }
        [JsonProperty(PropertyName = "_levelAuthorName")]
        public string LevelAuthorName { get; set; }
        [JsonProperty(PropertyName = "_beatsPerMinute")]
        public float BeatsPerMinute { get; set; }
        [JsonProperty(PropertyName = "_shuffle")]
        public float Shuffle { get; set; }
        [JsonProperty(PropertyName = "_shufflePeriod")]
        public float ShufflePeriod { get; set; }
        [JsonProperty(PropertyName = "_previewStartTime")]
        public float PreviewStartTime { get; set; }
        [JsonProperty(PropertyName = "_previewDuration")]
        public float PreviewDuration { get; set; }
        [JsonProperty(PropertyName = "_songFilename")]
        public string SongFilename { get; set; }
        [JsonProperty(PropertyName = "_coverImageFilename")]
        public string CoverImageFilename { get; set; }
        [JsonProperty(PropertyName = "_environmentName")]
        public string EnvironmentName { get; set; }
        [JsonProperty(PropertyName = "_allDirectionsEnvironmentName")]
        public string AllDirectionsEnvironmentName { get; set; }
        [JsonProperty(PropertyName = "_songTimeOffset")]
        public float SongTimeOffset { get; set; }
        [JsonProperty(PropertyName = "_environmentNames")]
        public List<string> EnvironmentNames { get; set; }
        [JsonProperty(PropertyName = "_colorSchemes")]
        public List<string> ColorSchemes { get; set; }
        [JsonProperty(PropertyName = "_customData")]
        public JObject CustomData { get; set; }
        [JsonProperty(PropertyName = "_difficultyBeatmapSets")]
        public List<DifficultyBeatmapSet> DifficultyBeatmapSets { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BeatmapInfo info &&
                   Version == info.Version &&
                   SongName == info.SongName &&
                   SongSubName == info.SongSubName &&
                   SongAuthorName == info.SongAuthorName &&
                   LevelAuthorName == info.LevelAuthorName &&
                   BeatsPerMinute == info.BeatsPerMinute &&
                   Shuffle == info.Shuffle &&
                   ShufflePeriod == info.ShufflePeriod &&
                   PreviewStartTime == info.PreviewStartTime &&
                   PreviewDuration == info.PreviewDuration &&
                   SongFilename == info.SongFilename &&
                   CoverImageFilename == info.CoverImageFilename &&
                   EnvironmentName == info.EnvironmentName &&
                   AllDirectionsEnvironmentName == info.AllDirectionsEnvironmentName &&
                   SongTimeOffset == info.SongTimeOffset &&
                   EqualityComparer<List<string>>.Default.Equals(EnvironmentNames, info.EnvironmentNames) &&
                   EqualityComparer<List<string>>.Default.Equals(ColorSchemes, info.ColorSchemes) &&
                   EqualityComparer<JObject>.Default.Equals(CustomData, info.CustomData) &&
                   EqualityComparer<List<DifficultyBeatmapSet>>.Default.Equals(DifficultyBeatmapSets, info.DifficultyBeatmapSets);
        }

        public override int GetHashCode()
        {
            int hashCode = 797719400;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Version);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SongName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SongSubName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SongAuthorName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LevelAuthorName);
            hashCode = hashCode * -1521134295 + BeatsPerMinute.GetHashCode();
            hashCode = hashCode * -1521134295 + Shuffle.GetHashCode();
            hashCode = hashCode * -1521134295 + ShufflePeriod.GetHashCode();
            hashCode = hashCode * -1521134295 + PreviewStartTime.GetHashCode();
            hashCode = hashCode * -1521134295 + PreviewDuration.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SongFilename);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CoverImageFilename);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EnvironmentName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AllDirectionsEnvironmentName);
            hashCode = hashCode * -1521134295 + SongTimeOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(EnvironmentNames);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(ColorSchemes);
            hashCode = hashCode * -1521134295 + EqualityComparer<JObject>.Default.GetHashCode(CustomData);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<DifficultyBeatmapSet>>.Default.GetHashCode(DifficultyBeatmapSets);
            return hashCode;
        }
    }
}
