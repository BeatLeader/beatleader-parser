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
using Parser.Audio.V4;
using Parser.Map.Difficulty.V4.Base;

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

                var info = Helper.DeserializeFromStream<Info>(infoFile.Open());
                if (info == null || info._difficultyBeatmapSets == null) {
                    var v4Info = Helper.DeserializeFromStream<Parser.Map.V4.Info>(infoFile.Open());
                    if (v4Info == null) {
                        return null;
                    }

                    v3.Info = v4Info.ToV2();
                    AudioData? audioData = null;

                    var audioDataFile = archive.Entries.FirstOrDefault(e => e.Name == v4Info.audio.audioDataFilename);
                    if (audioDataFile != null) {
                        audioData = Helper.DeserializeFromStream<AudioData>(audioDataFile.Open());
                    }

                    foreach (var beatmap in v4Info.difficultyBeatmaps)
                    {
                        var diffFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap.beatmapDataFilename);
                        if (diffFile == null) continue;

                        var diff = Helper.DeserializeFromStream<DifficultyV4>(diffFile.Open());
                        if (diff == null || diff.colorNotes == null) continue;

                        Lighting? lighting = null;
                        var lightsFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap.lightshowDataFilename);
                        if (lightsFile != null) {
                            lighting = Helper.DeserializeFromStream<Lighting>(lightsFile.Open());
                        }

                        var v3Diff = DifficultyV3.V4toV3(diff, audioData, lighting);
                        DifficultyV3.ConvertTime(v3Diff, v3.Info._beatsPerMinute);
                        DifficultyV3.CalculateObjectNjs(v3Diff, beatmap.noteJumpMovementSpeed);
                        v3.Difficulties.Add(new(beatmap.difficulty, beatmap.characteristic, v3Diff, beatmap.ToV2()));
                    }
                } else {
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
                                    var diff = Helper.DeserializeFromStream<DifficultyV2>(diffFile.Open());
                                    if (diff == null || diff._notes == null) continue;
                                    v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, DifficultyV3.V2toV3(diff, info._beatsPerMinute, beatmap._noteJumpMovementSpeed), beatmap));
                                }
                                else
                                {
                                    var diff = Helper.DeserializeFromStream<DifficultyV3>(diffFile.Open());
                                    if (diff == null || diff.Notes == null) continue;
                                    DifficultyV3.ConvertTime(diff, info._beatsPerMinute);
                                    DifficultyV3.CalculateObjectNjs(diff, beatmap._noteJumpMovementSpeed);
                                    v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff, beatmap));
                                }
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

                var infoJson = jsonStrings.Where(x => IsInfoFile(x.filename)).FirstOrDefault().json;
                var info = JsonConvert.DeserializeObject<Info>(infoJson);
                if (info == null || info._difficultyBeatmapSets == null) {
                    var v4Info = JsonConvert.DeserializeObject<Parser.Map.V4.Info>(infoJson);
                    if (v4Info == null) {
                        return null;
                    }

                    v3.Info = v4Info.ToV2();
                    AudioData? audioData = null;

                    var audioDataJson = jsonStrings.Where(x => x.filename == v4Info.audio.audioDataFilename).FirstOrDefault().json;
                    if (audioDataJson != null) {
                        audioData = JsonConvert.DeserializeObject<AudioData>(audioDataJson);
                    }

                    foreach (var beatmap in v4Info.difficultyBeatmaps)
                    {
                        var json = jsonStrings.Where(x => x.filename == beatmap.beatmapDataFilename).FirstOrDefault().json;
                        if (string.IsNullOrEmpty(json)) continue;

                        var diff = JsonConvert.DeserializeObject<DifficultyV4>(json);
                        if (diff == null || diff.colorNotes == null) continue;

                        Lighting? lighting = null;
                        var lightsFile = jsonStrings.Where(e => e.filename == beatmap.lightshowDataFilename).FirstOrDefault().json;
                        if (string.IsNullOrEmpty(lightsFile)) {
                            lighting = JsonConvert.DeserializeObject<Lighting>(lightsFile);
                        }

                        var v3Diff = DifficultyV3.V4toV3(diff, audioData, lighting);
                        DifficultyV3.ConvertTime(v3Diff, v3.Info._beatsPerMinute);
                        DifficultyV3.CalculateObjectNjs(v3Diff, beatmap.noteJumpMovementSpeed);
                        v3.Difficulties.Add(new(beatmap.difficulty, beatmap.characteristic, v3Diff, beatmap.ToV2()));
                    }
                } else {
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
                                    v3.Difficulties.Add(new(difficultyName, characteristicName, DifficultyV3.V2toV3(v2, info._beatsPerMinute, difficultyBeatmap._noteJumpMovementSpeed), difficultyBeatmap));
                                }
                            }
                            else
                            {
                                DifficultyV3 diffv3 = JsonConvert.DeserializeObject<DifficultyV3>(json);
                                DifficultyV3.ConvertTime(diffv3, info._beatsPerMinute);
                                DifficultyV3.CalculateObjectNjs(diffv3, difficultyBeatmap._noteJumpMovementSpeed);
                                if (v3 != null)
                                {
                                    v3.Difficulties.Add(new(difficultyName, characteristicName, diffv3, difficultyBeatmap));
                                }
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

                var info = Helper.DeserializeFromStream<Info>(infoFile.Open());
                if (info == null || info._difficultyBeatmapSets == null) {
                    var v4Info = Helper.DeserializeFromStream<Parser.Map.V4.Info>(infoFile.Open());
                    if (v4Info == null) {
                        return null;
                    }

                    v3.Info = v4Info.ToV2();
                    AudioData? audioData = null;

                    var audioDataFile = archive.Entries.FirstOrDefault(e => e.Name == v4Info.audio.audioDataFilename);
                    if (audioDataFile != null) {
                        audioData = Helper.DeserializeFromStream<AudioData>(audioDataFile.Open());
                    }

                    foreach (var beatmap in v4Info.difficultyBeatmaps)
                    {
                        var diffFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap.beatmapDataFilename);
                        if (diffFile == null) continue;

                        var diff = Helper.DeserializeFromStream<DifficultyV4>(diffFile.Open());
                        if (diff == null || diff.colorNotes == null) continue;

                        Lighting? lighting = null;
                        var lightsFile = archive.Entries.FirstOrDefault(e => e.Name == beatmap.lightshowDataFilename);
                        if (lightsFile != null) {
                            lighting = Helper.DeserializeFromStream<Lighting>(lightsFile.Open());
                        }

                        var v3Diff = DifficultyV3.V4toV3(diff, audioData, lighting);
                        DifficultyV3.ConvertTime(v3Diff, v3.Info._beatsPerMinute);
                        DifficultyV3.CalculateObjectNjs(v3Diff, beatmap.noteJumpMovementSpeed);
                        v3.Difficulties.Add(new(beatmap.difficulty, beatmap.characteristic, v3Diff, beatmap.ToV2()));
                    }
                } else {
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
                                var diff = Helper.DeserializeFromStream<DifficultyV2>(diffFile.Open());
                                if (diff == null || diff._notes == null) continue;
                                v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, DifficultyV3.V2toV3(diff, info._beatsPerMinute, beatmap._noteJumpMovementSpeed), beatmap));
                            }
                            else
                            {
                                var diff = Helper.DeserializeFromStream<DifficultyV3>(diffFile.Open());
                                if (diff == null || diff.Notes == null) continue;
                                DifficultyV3.ConvertTime(diff, info._beatsPerMinute);
                                DifficultyV3.CalculateObjectNjs(diff, beatmap._noteJumpMovementSpeed);
                                v3.Difficulties.Add(new(beatmap._difficulty, set._beatmapCharacteristicName, diff, beatmap));
                            }
                        }
                    }
                }

                var audioFile = archive.Entries.FirstOrDefault(e => e.Name.ToLower().EndsWith(".ogg") || e.Name.ToLower().EndsWith(".egg") || e.Name.ToLower().EndsWith(".wav"));
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
            AudioData? audioData = null;
            if (info == null || info._difficultyBeatmapSets == null) {
                var v4Info = JsonConvert.DeserializeObject<Parser.Map.V4.Info>(infoContent);
                if (v4Info == null) {
                    return null;
                }
                
                if (File.Exists($"{folderPath}/{v4Info.audio.audioDataFilename}")) {
                    audioData = JsonConvert.DeserializeObject<AudioData>(File.ReadAllText($"{folderPath}/{v4Info.audio.audioDataFilename}"));
                }

                info = v4Info.ToV2();
            }

            BeatmapV3 v3 = new()
            {
                Info = info
            };

            List<(string path, string lightingPath, string difficulty, string characteristic, _Difficultybeatmaps beatmap)> difficultyFiles = new();

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
                var text = File.ReadAllText($"{folderPath}/{diff.path}");
                if (text.Contains("_cutDirection") && !text.Contains("colorBoostBeatmapEvents"))
                {
                    DifficultyV2? v2 = JsonConvert.DeserializeObject<DifficultyV2>(File.ReadAllText($"{folderPath}/{diff.path}"));
                    if (v2 != null)
                    {
                        v3.Difficulties.Add(new(diff.difficulty, diff.characteristic, DifficultyV3.V2toV3(v2, info._beatsPerMinute, diff.beatmap._noteJumpMovementSpeed), diff.beatmap));
                    }
                }
                else if (text.Contains("colorNotesData"))
                {
                    DifficultyV3 diffv3 = DifficultyV3.V4toV3(
                        JsonConvert.DeserializeObject<DifficultyV4>(File.ReadAllText($"{folderPath}/{diff.path}")), 
                        audioData,
                        JsonConvert.DeserializeObject<Lighting>(File.ReadAllText($"{folderPath}/{diff.lightingPath}")));
                    DifficultyV3.ConvertTime(diffv3, info._beatsPerMinute);
                    DifficultyV3.CalculateObjectNjs(diffv3, diff.beatmap._noteJumpMovementSpeed);
                    if (v3 != null)
                    {
                        v3.Difficulties.Add(new(diff.difficulty, diff.characteristic, diffv3, diff.beatmap));
                    }
                }
                else
                {
                    DifficultyV3 diffv3 = JsonConvert.DeserializeObject<DifficultyV3>(File.ReadAllText($"{folderPath}/{diff.path}"));
                    DifficultyV3.ConvertTime(diffv3, info._beatsPerMinute);
                    DifficultyV3.CalculateObjectNjs(diffv3, diff.beatmap._noteJumpMovementSpeed);
                    if (v3 != null)
                    {
                        v3.Difficulties.Add(new(diff.difficulty, diff.characteristic, diffv3, diff.beatmap));
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

            return v3;
        }

        public SingleDiffBeatmapV3? TryLoadPath(string folderPath, string characteristic, string difficulty)
        {
            var infoContent = File.Exists($"{folderPath}/Info.dat") ? File.ReadAllText($"{folderPath}/Info.dat") : File.ReadAllText($"{folderPath}/info.dat");
            var info = JsonConvert.DeserializeObject<Info>(infoContent);
            AudioData? audioData = null;
            if (info == null || info._difficultyBeatmapSets == null) {
                var v4Info = JsonConvert.DeserializeObject<Parser.Map.V4.Info>(infoContent);
                if (v4Info == null) {
                    return null;
                }
                
                if (File.Exists($"{folderPath}/{v4Info.audio.audioDataFilename}")) {
                    audioData = JsonConvert.DeserializeObject<AudioData>(File.ReadAllText($"{folderPath}/{v4Info.audio.audioDataFilename}"));
                }

                info = v4Info.ToV2();
            }

            SingleDiffBeatmapV3 result = new()
            {
                Info = info
            };

            result.Info._difficultyBeatmapSets.RemoveAll(x => x._beatmapCharacteristicName != characteristic);
            result.Info._difficultyBeatmapSets.FirstOrDefault()._difficultyBeatmaps.RemoveAll(x => x._difficulty != difficulty);

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
                    var text = File.ReadAllText($"{folderPath}/{diff.path}");
                    if (text.Contains("_cutDirection") && !text.Contains("colorBoostBeatmapEvents"))
                    {
                        DifficultyV2? v2 = JsonConvert.DeserializeObject<DifficultyV2>(File.ReadAllText($"{folderPath}/{diff.path}"));
                        if (v2 != null)
                        {
                            result.Difficulty = (new(diff.difficulty, diff.characteristic, DifficultyV3.V2toV3(v2, info._beatsPerMinute, diff.beatMap._noteJumpMovementSpeed), diff.beatMap));
                        }
                    }
                    else if (text.Contains("colorNotesData"))
                    {
                        DifficultyV3 diffv3 = DifficultyV3.V4toV3(
                            JsonConvert.DeserializeObject<DifficultyV4>(File.ReadAllText($"{folderPath}/{diff.path}")), 
                            audioData,
                            JsonConvert.DeserializeObject<Lighting>(File.ReadAllText($"{folderPath}/{diff.lightingPath}")));
                        DifficultyV3.ConvertTime(diffv3, info._beatsPerMinute);
                        DifficultyV3.CalculateObjectNjs(diffv3, diff.beatMap._noteJumpMovementSpeed);
                        if (result != null)
                        {
                            result.Difficulty = (new(diff.difficulty, diff.characteristic, diffv3, diff.beatMap));
                        }
                    }
                    else
                    {
                        DifficultyV3 diffv3 = JsonConvert.DeserializeObject<DifficultyV3>(File.ReadAllText($"{folderPath}/{diff.path}"));
                        DifficultyV3.ConvertTime(diffv3, info._beatsPerMinute);
                        DifficultyV3.CalculateObjectNjs(diffv3, diff.beatMap._noteJumpMovementSpeed);
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
