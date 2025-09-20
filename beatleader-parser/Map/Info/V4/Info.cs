using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatMapParser.Map.V4 {
    public class InfoV4 {
        public string version { get; set; }
        public Song song { get; set; }
        public Audio audio { get; set; }
        public string songPreviewFilename { get; set; }
        public string coverImageFilename { get; set; }
        public string[] environmentNames { get; set; }
        public ColorScheme[] colorSchemes { get; set; }
        public DifficultyBeatmap[] difficultyBeatmaps { get; set; }

        public Map.Info ToV2() {
            return new Map.Info {
                _version = "2.1.0",
                _songName = song.title,
                _songSubName = song.subTitle ?? "",
                _songAuthorName = song.author,
                _levelAuthorName = difficultyBeatmaps.Length > 0 && difficultyBeatmaps[0].beatmapAuthors.mappers.Length > 0 ? difficultyBeatmaps[0].beatmapAuthors.mappers[0] : "",
                _beatsPerMinute = audio.bpm,
                _songTimeOffset = 0,
                _shuffle = 0,
                _shufflePeriod = 0,
                _previewStartTime = audio.previewStartTime,
                _previewDuration = audio.previewDuration,
                _songFilename = audio.songFilename,
                _coverImageFilename = coverImageFilename,
                _environmentName = environmentNames.Length > 0 ? environmentNames[0] : "TriangleEnvironment",
                _allDirectionsEnvironmentName = environmentNames.Length > 1 ? environmentNames[1] : "",
                _environmentNames = environmentNames,
                _colorSchemes = colorSchemes,
                _difficultyBeatmapSets = ConvertDifficultyBeatmaps()
            };
        }

        private List<_Difficultybeatmapsets> ConvertDifficultyBeatmaps() {
            var sets = new Dictionary<string, _Difficultybeatmapsets>();

            foreach (var diff in difficultyBeatmaps) {
                var characteristic = diff.characteristic ?? "Standard";
                
                if (!sets.ContainsKey(characteristic)) {
                    sets[characteristic] = new _Difficultybeatmapsets {
                        _beatmapCharacteristicName = characteristic,
                        _difficultyBeatmaps = new List<_Difficultybeatmaps>()
                    };
                }

                sets[characteristic]._difficultyBeatmaps.Add(diff.ToV2());
            }

            return sets.Values.ToList();
        }

        
    }

    public class Song {
        public string title { get; set; }
        public string subTitle { get; set; }
        public string author { get; set; }
    }

    public class Audio {
        public string songFilename { get; set; }
        public float songDuration { get; set; }
        public string audioDataFilename { get; set; }
        public float bpm { get; set; }
        public float lufs { get; set; }
        public float previewStartTime { get; set; }
        public float previewDuration { get; set; }
    }

    public class ColorScheme {
        public bool useOverride { get; set; }
        public string colorSchemeName { get; set; }
        public string saberAColor { get; set; }
        public string saberBColor { get; set; }
        public string obstaclesColor { get; set; }
        public string environmentColor0 { get; set; }
        public string environmentColor1 { get; set; }
        public string environmentColor0Boost { get; set; }
        public string environmentColor1Boost { get; set; }
    }

    public class BeatmapAuthors {
        public string[] mappers { get; set; }
        public string[] lighters { get; set; }
    }

    public class DifficultyBeatmap {
        public string characteristic { get; set; }
        public string difficulty { get; set; }
        public BeatmapAuthors beatmapAuthors { get; set; }
        public int environmentNameIdx { get; set; }
        public int beatmapColorSchemeIdx { get; set; }
        public float noteJumpMovementSpeed { get; set; }
        public float noteJumpStartBeatOffset { get; set; }
        public string beatmapDataFilename { get; set; }
        public string lightshowDataFilename { get; set; }

        private int GetDifficultyRank(string difficulty) {
            return difficulty switch {
                "Easy" => 1,
                "Normal" => 3,
                "Hard" => 5,
                "Expert" => 7,
                "ExpertPlus" => 9,
                _ => 1
            };
        }

        public _Difficultybeatmaps ToV2() {
            return new _Difficultybeatmaps {
                _difficulty = difficulty,
                _difficultyRank = GetDifficultyRank(difficulty),
                _beatmapFilename = beatmapDataFilename,
                _lightshowDataFilename = lightshowDataFilename,
                _noteJumpMovementSpeed = noteJumpMovementSpeed,
                _noteJumpStartBeatOffset = noteJumpStartBeatOffset,
                _beatmapColorSchemeIdx = beatmapColorSchemeIdx,
                _environmentNameIdx = environmentNameIdx
            };
        }
    }
}
