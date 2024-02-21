using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class DifficultyBeatmap
    {
        [JsonProperty(PropertyName = "_difficulty")]
        public Difficulty Difficulty { get; set; }
        [JsonProperty(PropertyName = "_difficultyRank")]
        public int DifficultyRank { get; set; }
        [JsonProperty(PropertyName = "_beatmapFilename")]
        public string BeatmapFilename { get; set; }
        [JsonProperty(PropertyName = "_noteJumpMovementSpeed")]
        public float NoteJumpMovementSpeed { get; set; }
        [JsonProperty(PropertyName = "_noteJumpStartBeatOffset")]
        public float NoteJumpStartBeatOffset { get; set; }
        [JsonProperty(PropertyName = "_beatmapColorSchemeIdx")]
        public int BeatmapColorSchemeIdx { get; set; }
        [JsonProperty(PropertyName = "_environmentNameIdx")]
        public int EnvironmentNameIdx { get; set; }
        [JsonProperty(PropertyName = "_customData")]
        public ExpandoObject CustomData { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DifficultyBeatmap beatmap &&
                   Difficulty == beatmap.Difficulty &&
                   DifficultyRank == beatmap.DifficultyRank &&
                   BeatmapFilename == beatmap.BeatmapFilename &&
                   NoteJumpMovementSpeed == beatmap.NoteJumpMovementSpeed &&
                   NoteJumpStartBeatOffset == beatmap.NoteJumpStartBeatOffset &&
                   BeatmapColorSchemeIdx == beatmap.BeatmapColorSchemeIdx &&
                   EnvironmentNameIdx == beatmap.EnvironmentNameIdx &&
                   EqualityComparer<ExpandoObject>.Default.Equals(CustomData, beatmap.CustomData);
        }

        public override int GetHashCode()
        {
            int hashCode = 711299465;
            hashCode = hashCode * -1521134295 + Difficulty.GetHashCode();
            hashCode = hashCode * -1521134295 + DifficultyRank.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BeatmapFilename);
            hashCode = hashCode * -1521134295 + NoteJumpMovementSpeed.GetHashCode();
            hashCode = hashCode * -1521134295 + NoteJumpStartBeatOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + BeatmapColorSchemeIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + EnvironmentNameIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ExpandoObject>.Default.GetHashCode(CustomData);
            return hashCode;
        }
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Expert,
        ExpertPlus
    }
}
