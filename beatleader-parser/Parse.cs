using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Net;
using Parser.Map;
using Parser.Map.Difficulty.V3.Base;
using Parser.Json;
using System.Linq;
using Parser.Map.Difficulty.V2.Base;
using Newtonsoft.Json;
using Parser.Audio;

namespace beatleader_parser
{
    public class Parse
    {
        internal List<string> CharacteristicName = new() { "Standard", "NoArrows", "OneSaber", "360Degree", "90Degree", "Legacy", "Lightshow", "Lawless" };

        internal bool IsInfoFile(string filename) {
            return filename.ToLower() == "info.dat";
        }

        public List<BeatmapV3> TryLoadZip(MemoryStream data)
        {
            try
            {
                List<BeatmapV3> map = new();
                BeatmapV3 v3 = new();

                List<(string fileName, DifficultyV3 diff)> difficulties = new();
                ZipArchive archive = new(data);
                var infoFile = archive.Entries.FirstOrDefault(e => IsInfoFile(e.Name));
                if (infoFile == null) return null;

                var info = Helper.DeserializeInfoFromStream(infoFile.Open());
                if (info == null) return null;

                v3.Info = info;
                foreach (var set in info._difficultyBeatmapSets)
                {
                    foreach (var beatmap in set._difficultyBeatmaps)
                    {
                        var diffFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap._beatmapFilename);
                        if (diffFile == null) continue;
                        using (StreamReader reader = new(diffFile.Open()))
                        {
                            string content = reader.ReadToEnd();
                            if (content.Contains("_cutDirection") && !content.Contains("colorBoostBeatmapEvents"))
                            {
                                var diff = Helper.DeserializeV2DiffFromStream(diffFile.Open(), info._beatsPerMinute);
                                if (diff == null || diff.Notes == null) continue;
                                v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff, beatmap));
                            }
                            else
                            {
                                var diff = Helper.DeserializeV3DiffFromStream(diffFile.Open());
                                if (diff == null || diff.Notes == null) continue;
                                DifficultyV3.ConvertTime(diff, info._beatsPerMinute);
                                v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff, beatmap));
                            }
                        }
                    }
                }
                var audioFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower().EndsWith(".ogg") || e.Name.ToLower().EndsWith(".egg"));
                if (audioFile == null) return null;

                Ogg ogg = new();
                using (var ms = new MemoryStream(5)) {
                    audioFile.Open().CopyTo(ms);
                    v3.SongLength = ogg.AudioStreamToLength(ms);
                }
                map.Add(v3);

                return map;
            }
            catch
            {
                return null;
            }
        }

        public List<BeatmapV3> TryLoadString(List<(string filename, string json)> jsonStrings, float songLength)
        {
            try
            {
                List<BeatmapV3> map = new();
                BeatmapV3 v3 = new();

                var info = JsonConvert.DeserializeObject<Info>(jsonStrings.Where(x => IsInfoFile(x.filename)).FirstOrDefault().json);
                if (info == null) return null;

                v3.Info = info;
                foreach (var characteristic in info._difficultyBeatmapSets)
                {
                    string characteristicName = characteristic._beatmapCharacteristicName;

                    foreach (var difficultyBeatmap in characteristic._difficultyBeatmaps)
                    {
                        string difficultyName = difficultyBeatmap._difficulty;
                        string json = jsonStrings.Where(x => x.filename == $"{difficultyName + characteristicName}.dat").FirstOrDefault().json;
                        if (json.Contains("_cutDirection") && !json.Contains("colorBoostBeatmapEvents"))
                        {
                            DifficultyV2 v2 = JsonConvert.DeserializeObject<DifficultyV2>(json);
                            if (v2 != null)
                            {
                                v3.Difficulties.Add(new(difficultyName, characteristicName, DifficultyV3.V2toV3(v2, info._beatsPerMinute), difficultyBeatmap));
                            }
                        }
                        else
                        {
                            DifficultyV3 diffv3 = JsonConvert.DeserializeObject<DifficultyV3>(json);
                            DifficultyV3.ConvertTime(diffv3, info._beatsPerMinute);
                            if (v3 != null)
                            {
                                v3.Difficulties.Add(new(difficultyName, characteristicName, diffv3, difficultyBeatmap));
                            }
                        }
                    }
                }

                v3.SongLength = songLength;
                map.Add(v3);

                return map;
            }
            catch
            {
                return null;
            }
        }

        public List<BeatmapV3> TryDownloadLink(string downloadLink)
        {
            try
            {
                List<BeatmapV3> map = new();
                BeatmapV3 v3 = new();

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

                var info = Helper.DeserializeInfoFromStream(infoFile.Open());
                if (info == null) return null;
                v3.Info = info;

                foreach (var set in info._difficultyBeatmapSets)
                {
                    foreach (var beatmap in set._difficultyBeatmaps)
                    {
                        var diffFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap._beatmapFilename);
                        if (diffFile == null) continue;
                        using StreamReader reader = new(diffFile.Open());
                        string content = reader.ReadToEnd();
                        if (content.Contains("_cutDirection") && !content.Contains("colorBoostBeatmapEvents"))
                        {
                            var diff = Helper.DeserializeV2DiffFromStream(diffFile.Open(), info._beatsPerMinute);
                            if (diff == null || diff.Notes == null) continue;
                            v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff, beatmap));
                        }
                        else
                        {
                            var diff = Helper.DeserializeV3DiffFromStream(diffFile.Open());
                            if (diff == null || diff.Notes == null) continue;
                            DifficultyV3.ConvertTime(diff, info._beatsPerMinute);
                            v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff, beatmap));
                        }
                    }
                }

                var audioFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower().EndsWith(".ogg") || e.Name.ToLower().EndsWith(".egg"));
                if (audioFile == null) return null;

                Ogg ogg = new();
                v3.SongLength = ogg.AudioStreamToLength(audioFile.Open());
                map.Add(v3);

                return map;
            }
            catch
            {
                return null;
            }
        }
        #nullable enable
        public BeatmapV3? TryLoadPath(string folderPath)
        {
            var infoContent = File.Exists($"{folderPath}/Info.dat") ? File.ReadAllText($"{folderPath}/Info.dat") : File.ReadAllText($"{folderPath}/info.dat");
            var info = JsonConvert.DeserializeObject<Info>(infoContent);
            if (info == null) return null;

            BeatmapV3 result = new()
            {
                Info = info
            };

            List<(string path, string difficulty, string characteristic, _Difficultybeatmaps beatmap)> difficultyFiles = new();

            foreach (var characteristics in info._difficultyBeatmapSets)
            {
                string characteristicName = characteristics._beatmapCharacteristicName;

                foreach (var difficultyBeatmap in characteristics._difficultyBeatmaps)
                {
                    string difficultyName = difficultyBeatmap._difficulty;
                    difficultyFiles.Add(new($"{difficultyBeatmap._beatmapFilename}", difficultyName, characteristicName, difficultyBeatmap));
                }
            }

            foreach (var diff in difficultyFiles)
            {
                var text = File.ReadAllText($"{folderPath}/{diff.path}");
                if (text.Contains("_cutDirection") && !text.Contains("colorBoostBeatmapEvents"))
                {
                    DifficultyV2? v2 = JsonConvert.DeserializeObject<DifficultyV2>(File.ReadAllText($"{folderPath}/{diff.path}"));
                    if (v2 != null)
                    {
                        result.Difficulties.Add(new(diff.difficulty, diff.characteristic, DifficultyV3.V2toV3(v2, info._beatsPerMinute), diff.beatmap));
                    }
                }
                else
                {
                    DifficultyV3 diffv3 = JsonConvert.DeserializeObject<DifficultyV3>(File.ReadAllText($"{folderPath}/{diff.path}"));
                    DifficultyV3.ConvertTime(diffv3, info._beatsPerMinute);
                    if (result != null)
                    {
                        result.Difficulties.Add(new(diff.difficulty, diff.characteristic, diffv3, diff.beatmap));
                    }
                }
            }

            var audioFilePath = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly).Where(f => f.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".egg", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (audioFilePath != null)
            {
                using var stream = File.OpenRead(audioFilePath);
                using var vorbis = new NVorbis.VorbisReader(stream);
                result.SongLength = (double)vorbis.TotalSamples / vorbis.SampleRate;
            }

            return result;
        }

        public SingleDiffBeatmapV3? TryLoadPath(string folderPath, string characteristic, string difficulty)
        {
            var infoContent = File.Exists($"{folderPath}/Info.dat") ? File.ReadAllText($"{folderPath}/Info.dat") : File.ReadAllText($"{folderPath}/info.dat");
            var info = JsonConvert.DeserializeObject<Info>(infoContent);
            if (info == null) return null;

            SingleDiffBeatmapV3 result = new()
            {
                Info = info
            };

            result.Info._difficultyBeatmapSets.RemoveAll(x => x._beatmapCharacteristicName != characteristic);
            result.Info._difficultyBeatmapSets.FirstOrDefault()._difficultyBeatmaps.RemoveAll(x => x._difficulty != difficulty);

            List<(string path, string difficulty, string characteristic, _Difficultybeatmaps beatMap)> difficultyFiles = new();

            foreach (var characteristics in info._difficultyBeatmapSets)
            {
                string characteristicName = characteristics._beatmapCharacteristicName;

                foreach (var difficultyBeatmap in characteristics._difficultyBeatmaps)
                {
                    string difficultyName = difficultyBeatmap._difficulty;
                    difficultyFiles.Add(new($"{difficultyBeatmap._beatmapFilename}", difficultyName, characteristicName, difficultyBeatmap));
                }
            }

            foreach (var diff in difficultyFiles)
            {
                if (diff.characteristic == characteristic && diff.difficulty == difficulty)
                {
                    var text = File.ReadAllText($"{folderPath}/{diff.path}");
                    if (text.Contains("_cutDirection") && !text.Contains("colorBoostBeatmapEvents"))
                    {
                        DifficultyV2? v2 = JsonConvert.DeserializeObject<DifficultyV2>(File.ReadAllText($"{folderPath}/{diff.path}"));
                        if (v2 != null)
                        {
                            result.Difficulty = (new(diff.difficulty, diff.characteristic, DifficultyV3.V2toV3(v2, info._beatsPerMinute), diff.beatMap));
                        }
                    }
                    else
                    {
                        DifficultyV3 diffv3 = JsonConvert.DeserializeObject<DifficultyV3>(File.ReadAllText($"{folderPath}/{diff.path}"));
                        DifficultyV3.ConvertTime(diffv3, info._beatsPerMinute);
                        if (result != null)
                        {
                            result.Difficulty = (new(diff.difficulty, diff.characteristic, diffv3, diff.beatMap));
                        }
                    }
                }
            }

            var audioFilePath = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly).Where(f => f.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".egg", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (audioFilePath != null)
            {
                using var stream = File.OpenRead(audioFilePath);
                using var vorbis = new NVorbis.VorbisReader(stream);
                result.SongLength = (double)vorbis.TotalSamples / vorbis.SampleRate;
            }

            return result;
        }
    }
}
