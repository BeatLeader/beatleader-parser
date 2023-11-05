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
    public static class Parse
    {
        internal static List<string> CharacteristicName = new() { "Standard", "NoArrows", "OneSaber", "360Degree", "90Degree", "Legacy", "Lightshow", "Lawless" };

        public static bool TryLoadZip(MemoryStream data)
        {
            BeatmapV3.Reset();

            try
            {
                List<(string fileName, DifficultyV3 diff)> difficulties = new();
                ZipArchive archive = new(data);
                var infoFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower() == "info.dat");
                if (infoFile == null) return false;

                var info = Helper.DeserializeInfoFromStream(infoFile.Open());
                BeatmapV3.Instance.Info = info;
                if (info == null) return false;

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
                                BeatmapV3.Instance.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff));
                            }
                            else
                            {
                                var diff = Helper.DeserializeV3DiffFromStream(diffFile.Open());
                                if (diff == null || diff.colorNotes == null) continue;
                                BeatmapV3.Instance.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff));
                            }
                        }
                    }
                }
                var audioFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower().EndsWith(".ogg") || e.Name.ToLower().EndsWith(".egg"));
                if (audioFile == null) return false;

                Ogg ogg = new();
                BeatmapV3.Instance.SongLength = ogg.AudioStreamToLength(audioFile.Open());

                return true;
            }
            catch
            {
                BeatmapV3.Reset();
                return false;
            }
        }

        public static bool TryLoadString(List<(string filename, string json)> jsonStrings, float songLength)
        {
            BeatmapV3.Reset();

            try
            {
                BeatmapV3.Instance.Info = JsonConvert.DeserializeObject<Info>(jsonStrings.Where(x => x.filename == "Info.json").FirstOrDefault().json);

                foreach (var characteristic in BeatmapV3.Instance.Info._difficultyBeatmapSets)
                {
                    string characteristicName = characteristic._beatmapCharacteristicName;

                    foreach (var difficultyBeatmap in characteristic._difficultyBeatmaps)
                    {
                        string difficultyName = difficultyBeatmap._difficulty;
                        string json = jsonStrings.Where(x => x.filename == $"{difficultyName + characteristicName}.dat").FirstOrDefault().json;
                        if (json.Contains("_version\":\"2"))
                        {
                            DifficultyV2? v2 = JsonConvert.DeserializeObject<DifficultyV2>(json);
                            if (v2 != null)
                            {
                                BeatmapV3.Instance.Difficulties.Add(new(difficultyName, characteristicName, DifficultyV3.V2toV3(v2)));
                            }
                        }
                        else
                        {
                            DifficultyV3? v3 = JsonConvert.DeserializeObject<DifficultyV3>(json);
                            if (v3 != null)
                            {
                                BeatmapV3.Instance.Difficulties.Add(new(difficultyName, characteristicName, v3));
                            }
                        }
                    }
                }

                BeatmapV3.Instance.SongLength = songLength;

                return true;
            }
            catch
            {
                BeatmapV3.Reset();
                return false;
            }
        }

        public static bool TryDownloadLink(string downloadLink)
        {
            BeatmapV3.Reset();

            try
            {
                HttpWebResponse? res = null;
                try
                {
                    res = (HttpWebResponse)WebRequest.Create(downloadLink).GetResponse();
                }
                catch { }
                if (res == null || res.StatusCode != HttpStatusCode.OK) return false;

                var archive = new ZipArchive(res.GetResponseStream());

                var infoFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower() == "info.dat");
                if (infoFile == null) return false;

                var info = Helper.DeserializeInfoFromStream(infoFile.Open());
                BeatmapV3.Instance.Info = info;
                if (info == null) return false;

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
                                BeatmapV3.Instance.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff));
                            }
                            else
                            {
                                var diff = Helper.DeserializeV3DiffFromStream(diffFile.Open());
                                if (diff == null || diff.colorNotes == null) continue;
                                BeatmapV3.Instance.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff));
                            }
                        }
                    }
                }

                var audioFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower().EndsWith(".ogg") || e.Name.ToLower().EndsWith(".egg"));
                if (audioFile == null) return false;

                Ogg ogg = new();
                BeatmapV3.Instance.SongLength = ogg.AudioStreamToLength(audioFile.Open());

                return true;
            }
            catch
            {
                BeatmapV3.Reset();
                return false;
            }
        }

        public static bool TryLoadPath(string folderPath)
        {
            BeatmapV3.Reset();

            try
            {
                BeatmapV3.Instance.Info = JsonConvert.DeserializeObject<Info>(File.ReadAllText($"{folderPath}/Info.dat"));
                if (BeatmapV3.Instance.Info != null)
                {
                    List<(string path, string difficulty, string characteristic)> difficultyFiles = new();

                    foreach (var characteristic in BeatmapV3.Instance.Info._difficultyBeatmapSets)
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
                            DifficultyV2? v2 = JsonConvert.DeserializeObject<DifficultyV2>(File.ReadAllText($"{folderPath}/{difficulty.path}"));
                            if (v2 != null)
                            {
                                BeatmapV3.Instance.Difficulties.Add(new(difficulty.difficulty, difficulty.characteristic, DifficultyV3.V2toV3(v2)));
                            }
                        }
                        else
                        {
                            DifficultyV3? v3 = JsonConvert.DeserializeObject<DifficultyV3>(File.ReadAllText($"{folderPath}/{difficulty.path}"));
                            if (v3 != null)
                            {
                                BeatmapV3.Instance.Difficulties.Add(new(difficulty.difficulty, difficulty.characteristic, v3));
                            }
                        }
                    }

                    var audioFilePath = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly).Where(f => f.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".egg", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (audioFilePath != null)
                    {
                        using var stream = File.OpenRead(audioFilePath);
                        using var vorbis = new NVorbis.VorbisReader(stream);
                        BeatmapV3.Instance.SongLength = (double)vorbis.TotalSamples / vorbis.SampleRate;
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                BeatmapV3.Reset();
                return false;
            }
        }

        public static BeatmapV3? GetBeatmap()
        {
            if (BeatmapV3.Instance.Info != null)
            {
                return BeatmapV3.Instance;
            }
            return null;
        }
    }
}
