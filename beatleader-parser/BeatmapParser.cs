using beatleader_parser.Beatmap;
using Newtonsoft.Json.Linq;
using Parser.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace beatleader_parser
{
    public class BeatmapParser
    {
        /// <summary>
        /// Load a beatmap from a path to a map directory.
        /// </summary>
        /// <param name="path">The path pointing to the map directory</param>
        /// <returns><see cref="BeatmapData"/></returns>
        public static BeatmapData LoadPath(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();
            string infoPath = path + "/info.dat";

            BeatmapInfo beatmapInfo = LoadInfo(infoPath);

            List<BeatmapDifficultyEntry> difficultyBeatmapSets = LoadDifficultiesFromPath(beatmapInfo, path);

            return new BeatmapData(beatmapInfo, difficultyBeatmapSets);
        }

        public static BeatmapInfo LoadInfo(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("info.dat not found in the map directory");
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return Helper.DeserializeInfoFromStream(stream);
            }
        }


        public static List<BeatmapDifficultyEntry> LoadDifficultiesFromPath(BeatmapInfo beatmapInfo, string directoryPath)
        {
            if (beatmapInfo == null) throw new ArgumentNullException(nameof(beatmapInfo));
            if (!Directory.Exists(directoryPath)) throw new DirectoryNotFoundException();

            List<BeatmapDifficultyEntry> difficultyEntries = new();

            foreach (var set in beatmapInfo.DifficultyBeatmapSets)
            {
                foreach (var diff in set.DifficultyBeatmaps)
                {
                    difficultyEntries.Add(ProcessDiffEntry(diff, directoryPath));
                }
            }

            return difficultyEntries;
        }

        private static BeatmapDifficultyEntry ProcessDiffEntry(DifficultyBeatmap diff, string directoryPath)
        {
            string path = directoryPath + "/" + diff.BeatmapFilename;
            if (!File.Exists(path)) throw new FileNotFoundException($"Difficulty at {path} not found.");

            JObject json = JObject.Parse(File.ReadAllText(path));
            string version = json["version"].ToString() ?? json["_version"].ToString() ?? throw new Exception("Json does not contain a version number.");

            if (Helper.IsBetweenVersions("3.0.0", "4.0.0", version))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    BeatmapDifficulty difficulty = Helper.DeserializeV3DiffFromStream(stream);
                    return new()
                    {
                        Difficulty = diff.Difficulty,
                        DifficultyRank = diff.DifficultyRank,
                        BeatmapDifficulty = difficulty
                    };
                }
            }
            else
            {
                throw new NotImplementedException("Difficulty version not implemented");
            }
        }
    }
}
