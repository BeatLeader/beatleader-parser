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

        public List<BeatmapV3> TryLoadZip(MemoryStream data)
        {
            try
            {
                List<BeatmapV3> map = new();
                BeatmapV3 v3 = new();

                List<(string fileName, DifficultyV3 diff)> difficulties = new();
                ZipArchive archive = new(data);
                var infoFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower() == "info.dat");
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
                            if (content.Contains("_version\":\"2"))
                            {
                                var diff = Helper.DeserializeV2DiffFromStream(diffFile.Open());
                                if (diff == null || diff.colorNotes == null) continue;
                                v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff));
                            }
                            else
                            {
                                var diff = Helper.DeserializeV3DiffFromStream(diffFile.Open());
                                if (diff == null || diff.colorNotes == null) continue;
                                v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff));
                            }
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

        public List<BeatmapV3> TryLoadString(List<(string filename, string json)> jsonStrings, float songLength)
        {
            try
            {
                List<BeatmapV3> map = new();
                BeatmapV3 v3 = new();

                var info = JsonConvert.DeserializeObject<Info>(jsonStrings.Where(x => x.filename == "Info.json").FirstOrDefault().json);
                if (info == null) return null;

                v3.Info = info;
                foreach (var characteristic in info._difficultyBeatmapSets)
                {
                    string characteristicName = characteristic._beatmapCharacteristicName;

                    foreach (var difficultyBeatmap in characteristic._difficultyBeatmaps)
                    {
                        string difficultyName = difficultyBeatmap._difficulty;
                        string json = jsonStrings.Where(x => x.filename == $"{difficultyName + characteristicName}.dat").FirstOrDefault().json;
                        if (json.Contains("_version\":\"2"))
                        {
                            DifficultyV2 v2 = JsonConvert.DeserializeObject<DifficultyV2>(json);
                            if (v2 != null)
                            {
                                v3.Difficulties.Add(new(difficultyName, characteristicName, DifficultyV3.V2toV3(v2)));
                            }
                        }
                        else
                        {
                            DifficultyV3 diffv3 = JsonConvert.DeserializeObject<DifficultyV3>(json);
                            if (v3 != null)
                            {
                                v3.Difficulties.Add(new(difficultyName, characteristicName, diffv3));
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

                var infoFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower() == "info.dat");
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
                        using (StreamReader reader = new StreamReader(diffFile.Open()))
                        {
                            string content = reader.ReadToEnd();
                            if (content.Contains("_version\":\"2"))
                            {
                                var diff = Helper.DeserializeV2DiffFromStream(diffFile.Open());
                                if (diff == null || diff.colorNotes == null) continue;
                                v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff));
                            }
                            else
                            {
                                var diff = Helper.DeserializeV3DiffFromStream(diffFile.Open());
                                if (diff == null || diff.colorNotes == null) continue;
                                v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff));
                            }
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

        public List<BeatmapV3> TryLoadPath(string folderPath)
        {
            try
            {
                List<BeatmapV3> map = new();
                BeatmapV3 v3 = new();

                var info = JsonConvert.DeserializeObject<Info>(File.ReadAllText($"{folderPath}/Info.dat"));
                if (info != null)
                {
                    v3.Info = info;

                    List<(string path, string difficulty, string characteristic)> difficultyFiles = new();

                    foreach (var characteristic in info._difficultyBeatmapSets)
                    {
                        string characteristicName = characteristic._beatmapCharacteristicName;

                        foreach (var difficultyBeatmap in characteristic._difficultyBeatmaps)
                        {
                            string difficultyName = difficultyBeatmap._difficulty;
                            difficultyFiles.Add(new($"{difficultyBeatmap._beatmapFilename}", difficultyName, characteristicName));
                        }
                    }

                    foreach (var difficulty in difficultyFiles)
                    {
                        if (File.ReadAllText($"{folderPath}/{difficulty.path}").Contains("_version\":\"2"))
                        {
                            DifficultyV2 v2 = JsonConvert.DeserializeObject<DifficultyV2>(File.ReadAllText($"{folderPath}/{difficulty.path}"));
                            if (v2 != null)
                            {
                                v3.Difficulties.Add(new(difficulty.difficulty, difficulty.characteristic, DifficultyV3.V2toV3(v2)));
                            }
                        }
                        else
                        {
                            DifficultyV3 diffv3 = JsonConvert.DeserializeObject<DifficultyV3>(File.ReadAllText($"{folderPath}/{difficulty.path}"));
                            if (v3 != null)
                            {
                                v3.Difficulties.Add(new(difficulty.difficulty, difficulty.characteristic, diffv3));
                            }
                        }
                    }

                    var audioFilePath = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly).Where(f => f.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".egg", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (audioFilePath != null)
                    {
                        using var stream = File.OpenRead(audioFilePath);
                        using var vorbis = new NVorbis.VorbisReader(stream);
                        v3.SongLength = (double)vorbis.TotalSamples / vorbis.SampleRate;
                    }

                    map.Add(v3);

                    return map;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
