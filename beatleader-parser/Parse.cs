using beatleader_parser.Utils;
using Parser.Audio;
using Parser.Audio.V4;
using Parser.Json;
using Parser.Map;
using Parser.Map.Difficulty.V2.Base;
using Parser.Map.Difficulty.V3.Base;
using Parser.Map.Difficulty.V4.Base;
using Parser.Map.V4;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace beatleader_parser
{
    public class Parse
    {
        internal List<string> CharacteristicName = new() { "Standard", "NoArrows", "OneSaber", "360Degree", "90Degree", "Legacy", "Lightshow", "Lawless" };

        internal bool IsInfoFile(string filename) {
            return filename.ToLower() == "info.dat";
        }

        private static (Info?, AudioData?) ParseInfo(string infoJson, ZipArchive audio)
        {
            var info = JsonSerializer.Deserialize<Info>(infoJson, SerializeV3Context.Default.Info);
            AudioData? audioData = null;

            if (info == null || info._difficultyBeatmapSets == null)
            {
                var v4Info = JsonSerializer.Deserialize<InfoV4>(infoJson, SerializeV4Context.Default.InfoV4);
                if (v4Info == null)
                {
                    return (null, null);
                }
                var audioDataFile = audio.Entries.FirstOrDefault(e => e.Name == v4Info.audio.audioDataFilename);
                if (audioDataFile != null)
                {
                    audioData = Helper.DeserializeFromStream<AudioData>(audioDataFile.Open(), SerializeV4Context.Default.AudioData);
                }
                info = v4Info.ToV2();
            }

            return (info, audioData);
        }

        private static (Info?, AudioData?) ParseInfo(string infoJson, List<(string filename, string json)> jsonStrings)
        {
            var info = JsonSerializer.Deserialize<Info>(infoJson, SerializeV3Context.Default.Info);
            AudioData? audioData = null;

            if (info == null || info._difficultyBeatmapSets == null)
            {
                var v4Info = JsonSerializer.Deserialize<InfoV4>(infoJson, SerializeV4Context.Default.InfoV4);
                if (v4Info == null)
                {
                    return (null, null);
                }
                var audioDataEntry = jsonStrings.FirstOrDefault(e => e.filename == v4Info.audio.audioDataFilename);
                if (audioDataEntry.filename != null)
                {
                    audioData = JsonSerializer.Deserialize<AudioData>(audioDataEntry.json, SerializeV4Context.Default.AudioData);
                }
                info = v4Info.ToV2();
            }

            return (info, audioData);
        }

        private static (Info?, AudioData?) ParseInfo(string infoJson, string folderPath, bool audioJson = false)
        {
            var info = JsonSerializer.Deserialize<Info>(infoJson, SerializeV3Context.Default.Info);
            AudioData? audioData = null;

            if (info == null || info._difficultyBeatmapSets == null)
            {
                var v4Info = JsonSerializer.Deserialize<InfoV4>(infoJson, SerializeV4Context.Default.InfoV4);
                if (v4Info == null)
                {
                    return (null, null);
                }
                if (audioJson)
                {
                    audioData = JsonSerializer.Deserialize<AudioData>(File.ReadAllText($"{folderPath}"), SerializeV4Context.Default.AudioData);
                }
                else
                {
                    if (File.Exists($"{folderPath}/{v4Info.audio.audioDataFilename}"))
                    {
                        audioData = JsonSerializer.Deserialize<AudioData>(File.ReadAllText($"{folderPath}/{v4Info.audio.audioDataFilename}"), SerializeV4Context.Default.AudioData);
                    }

                }
                info = v4Info.ToV2();
            }

            return (info, audioData);
        }

        private static void ProcessDifficulty(BeatmapV3 v3, string diffJson, string lightJson, AudioData? audioData, 
            string difficulty, string characteristic, _Difficultybeatmaps beatmapInfo, float bpm, float njs)
        {
            if (diffJson.Contains("_cutDirection") && !diffJson.Contains("colorBoostBeatmapEvents"))
            {
                var diff = JsonSerializer.Deserialize<DifficultyV2>(diffJson, SerializeV2Context.Default.DifficultyV2);
                if (diff != null)
                {
                    v3.Difficulties.Add(new(difficulty, characteristic, DifficultyV3.V2toV3(diff, bpm, njs), beatmapInfo));
                }
            }
            else if (diffJson.Contains("colorNotesData"))
            {
                var diff = JsonSerializer.Deserialize<DifficultyV4>(diffJson, SerializeV4Context.Default.DifficultyV4);
                if (diff == null || diff.colorNotes == null) return;

                Lighting? lighting = null;
                if (!string.IsNullOrEmpty(lightJson))
                {
                    lighting = JsonSerializer.Deserialize<Lighting>(lightJson, SerializeV4Context.Default.Lighting);
                }

                DifficultyV3 diffv3 = DifficultyV3.V4toV3(diff, audioData, lighting);
                DifficultyV3.ConvertTime(diffv3, bpm);
                DifficultyV3.CalculateObjectNjs(diffv3, njs);
                v3.Difficulties.Add(new(difficulty, characteristic, diffv3, beatmapInfo));
            }
            else
            {
                var diff = JsonSerializer.Deserialize<DifficultyV3>(diffJson, SerializeV3Context.Default.DifficultyV3);
                if (diff == null || diff.Notes == null) return;
                DifficultyV3.ConvertTime(diff, bpm);
                DifficultyV3.CalculateObjectNjs(diff, njs);
                v3.Difficulties.Add(new(difficulty, characteristic, diff, beatmapInfo));
            }
        }

        public BeatmapV3? TryLoadZip(MemoryStream data)
        {
            try
            {
                var archive = new ZipArchive(data, ZipArchiveMode.Read);

                var infoFile = archive.Entries.FirstOrDefault(e => IsInfoFile(e.Name));
                if (infoFile == null) return null;

                using var infoReader = new StreamReader(infoFile.Open());
                string infoJson = infoReader.ReadToEnd();

                var (info, audioData) = ParseInfo(infoJson, archive);

                if (info == null) return null;

                BeatmapV3 v3 = new()
                {
                    Info = info
                };

                foreach (var set in info._difficultyBeatmapSets)
                {
                    foreach (var beatmap in set._difficultyBeatmaps)
                    {
                        var diffFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap._beatmapFilename);
                        if (diffFile == null) continue;

                        using var diffReader = new StreamReader(diffFile.Open());
                        string diffJson = diffReader.ReadToEnd();

                        string lightJson = null;
                        var lightsFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap._lightshowDataFilename);
                        if (lightsFile != null)
                        {
                            using var lightReader = new StreamReader(lightsFile.Open());
                            lightJson = lightReader.ReadToEnd();
                        }

                        ProcessDifficulty(v3, diffJson, lightJson, audioData, beatmap._difficulty,
                            set._beatmapCharacteristicName, beatmap, info._beatsPerMinute, beatmap._noteJumpMovementSpeed);
                    }
                }

                var audioFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower().EndsWith(".ogg") || e.Name.ToLower().EndsWith(".egg") || e.Name.ToLower().EndsWith(".wav"));
                if (audioFile == null) return null;

                Ogg ogg = new();
                v3.SongLength = ogg.AudioStreamToLength(audioFile.Open());

                return v3;
            }
            catch
            {
                return null;
            }
        }

        public BeatmapV3? TryLoadString(List<(string filename, string json)> jsonStrings, float songLength)
        {
            try
            {
                var infoEntry = jsonStrings.FirstOrDefault(e => IsInfoFile(e.filename));
                if (infoEntry.filename == null) return null;

                var (info, audioData) = ParseInfo(infoEntry.json, jsonStrings);

                if (info == null) return null;

                BeatmapV3 v3 = new()
                {
                    Info = info,
                    SongLength = songLength
                };

                foreach (var set in info._difficultyBeatmapSets)
                {
                    foreach (var beatmap in set._difficultyBeatmaps)
                    {
                        var diffEntry = jsonStrings.FirstOrDefault(e => e.filename == beatmap._beatmapFilename);
                        if (diffEntry.filename == null) continue;

                        string lightJson = null;
                        var lightsEntry = jsonStrings.FirstOrDefault(e => e.filename == beatmap._lightshowDataFilename);
                        if (lightsEntry.filename != null)
                        {
                            lightJson = lightsEntry.json;
                        }

                        ProcessDifficulty(v3, diffEntry.json, lightJson, audioData, beatmap._difficulty,
                            set._beatmapCharacteristicName, beatmap, info._beatsPerMinute, beatmap._noteJumpMovementSpeed);
                    }
                }

                return v3;
            }
            catch
            {
                return null;
            }
        }

        public BeatmapV3? TryLoadDifficulty(string infoJson, string diffJson, string audioJson, string lightJson, float bpm, float njs, string characteristic, string difficulty)
        {
            try
            {
                var (info, audioData) = ParseInfo(infoJson, audioJson);
                if (info == null) return null;

                BeatmapV3 v3 = new()
                {
                    Info = info
                };

                // Create a dummy beatmap info for ProcessDifficulty
                _Difficultybeatmaps beatmapInfo = new()
                {
                    _noteJumpMovementSpeed = njs
                };

                ProcessDifficulty(v3, diffJson, lightJson, audioData,
                    difficulty, characteristic, beatmapInfo, bpm, njs);

                return v3;
            }
            catch
            {
                return null;
            }
        }

        #nullable enable
        public BeatmapV3? TryDownloadLink(string downloadLink)
        {
            try
            {
                HttpWebResponse res = null;
                try
                {
                    res = (HttpWebResponse)WebRequest.Create(downloadLink).GetResponse();
                }
                catch { }
                if (res == null || res.StatusCode != HttpStatusCode.OK) return null;

                var archive = new ZipArchive(res.GetResponseStream());

                var infoFile = archive.Entries.FirstOrDefault(e => IsInfoFile(e.Name));
                if (infoFile == null) return null;

                using var infoReader = new StreamReader(infoFile.Open());
                string infoJson = infoReader.ReadToEnd();

                var (info, audioData) = ParseInfo(infoJson, archive);

                if (info == null) return null;

                BeatmapV3 v3 = new()
                {
                    Info = info
                };

                foreach (var set in info._difficultyBeatmapSets)
                {
                    foreach (var beatmap in set._difficultyBeatmaps)
                    {
                        var diffFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap._beatmapFilename);
                        if (diffFile == null) continue;

                        using var diffReader = new StreamReader(diffFile.Open());
                        string diffJson = diffReader.ReadToEnd();

                        string lightJson = null;
                        var lightsFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap._lightshowDataFilename);
                        if (lightsFile != null)
                        {
                            using var lightReader = new StreamReader(lightsFile.Open());
                            lightJson = lightReader.ReadToEnd();
                        }

                        ProcessDifficulty(v3, diffJson, lightJson, audioData, beatmap._difficulty,
                            set._beatmapCharacteristicName, beatmap, info._beatsPerMinute, beatmap._noteJumpMovementSpeed);
                    }
                }

                var audioFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower().EndsWith(".ogg") || e.Name.ToLower().EndsWith(".egg") || e.Name.ToLower().EndsWith(".wav"));
                if (audioFile == null) return null;

                Ogg ogg = new();
                v3.SongLength = ogg.AudioStreamToLength(audioFile.Open());

                return v3;
            }
            catch
            {
                return null;
            }
        }
        #nullable enable
        public BeatmapV3? TryLoadPath(string folderPath)
        {
            try
            {
                var infoContent = File.Exists($"{folderPath}/Info.dat") ? File.ReadAllText($"{folderPath}/Info.dat") : File.ReadAllText($"{folderPath}/info.dat");

                var (info, audioData) = ParseInfo(infoContent, folderPath);
                if (info == null) return null;

                BeatmapV3 v3 = new()
                {
                    Info = info
                };

                foreach (var set in info._difficultyBeatmapSets)
                {
                    foreach (var beatmap in set._difficultyBeatmaps)
                    {
                        var diffPath = $"{folderPath}/{beatmap._beatmapFilename}";
                        if (!File.Exists(diffPath)) continue;

                        string diffJson = File.ReadAllText(diffPath);

                        string lightJson = null;
                        var lightPath = $"{folderPath}/{beatmap._lightshowDataFilename}";
                        if (File.Exists(lightPath))
                        {
                            lightJson = File.ReadAllText(lightPath);
                        }

                        ProcessDifficulty(v3, diffJson, lightJson, audioData, beatmap._difficulty,
                            set._beatmapCharacteristicName, beatmap, info._beatsPerMinute, beatmap._noteJumpMovementSpeed);
                    }
                }

                var audioFilePath = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly).Where(f => f.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".egg", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (audioFilePath != null)
                {
                    using var stream = File.OpenRead(audioFilePath);
                    using var vorbis = new NVorbis.VorbisReader(stream);
                    v3.SongLength = (double)vorbis.TotalSamples / vorbis.SampleRate;
                }

                return v3;
            }
            catch
            {
                return null;
            }
        }

        public BeatmapV3? TryLoadPath(string folderPath, string characteristic, string difficulty)
        {
            try
            {
                var infoContent = File.Exists($"{folderPath}/Info.dat") ? File.ReadAllText($"{folderPath}/Info.dat") : File.ReadAllText($"{folderPath}/info.dat");

                var (info, audioData) = ParseInfo(infoContent, folderPath);

                if (info == null) return null;

                BeatmapV3 v3 = new()
                {
                    Info = info
                };

                v3.Info._difficultyBeatmapSets.RemoveAll(x => x._beatmapCharacteristicName != characteristic);
                v3.Info._difficultyBeatmapSets.FirstOrDefault()._difficultyBeatmaps.RemoveAll(x => x._difficulty != difficulty);

                List<(string path, string lightingPath, string difficulty, string characteristic, _Difficultybeatmaps beatMap)> difficultyFiles = new();

                foreach (var characteristics in info._difficultyBeatmapSets)
                {
                    string characteristicName = characteristics._beatmapCharacteristicName;

                    foreach (var difficultyBeatmap in characteristics._difficultyBeatmaps)
                    {
                        string difficultyName = difficultyBeatmap._difficulty;
                        difficultyFiles.Add(new($"{difficultyBeatmap._beatmapFilename}", $"{difficultyBeatmap._lightshowDataFilename}", difficultyName, characteristicName, difficultyBeatmap));
                    }
                }

                foreach (var diff in difficultyFiles)
                {
                    if (diff.characteristic == characteristic && diff.difficulty == difficulty)
                    {
                        var diffPath = $"{folderPath}/{diff.beatMap._beatmapFilename}";
                        if (!File.Exists(diffPath)) continue;

                        string diffJson = File.ReadAllText(diffPath);

                        string lightJson = null;
                        var lightPath = $"{folderPath}/{diff.beatMap._lightshowDataFilename}";
                        if (File.Exists(lightPath))
                        {
                            lightJson = File.ReadAllText(lightPath);
                        }

                        ProcessDifficulty(v3, diffJson, lightJson, audioData, diff.beatMap._difficulty,
                            diff.characteristic, diff.beatMap, info._beatsPerMinute, diff.beatMap._noteJumpMovementSpeed);
                    }
                }

                var audioFilePath = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly).Where(f => f.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".egg", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (audioFilePath != null)
                {
                    using var stream = File.OpenRead(audioFilePath);
                    using var vorbis = new NVorbis.VorbisReader(stream);
                    v3.SongLength = (double)vorbis.TotalSamples / vorbis.SampleRate;
                }

                return v3;
            }
            catch
            {
                return null;
            }
        }
    }
}
