using Newtonsoft.Json;
using System.IO;
using beatleader_parser.Beatmap;
using System.Linq;
using System;

namespace Parser.Json
{
    internal class Helper
    {
        internal static BeatmapInfo DeserializeInfoFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();
            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);
            return serializer.Deserialize<BeatmapInfo>(jsonTextReader);
        }

        internal static BeatmapDifficulty DeserializeV3DiffFromStream(Stream stream)
        {
            try
            {
                var serializer = new JsonSerializer();
                using var sr = new StreamReader(stream);
                using var jsonTextReader = new JsonTextReader(sr);
                return serializer.Deserialize<BeatmapDifficulty>(jsonTextReader);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*internal static DifficultyV3 DeserializeV2DiffFromStream(Stream stream, float bpm)
        {
            try
            {
                var serializer = new JsonSerializer();
                using var sr = new StreamReader(stream);
                using var jsonTextReader = new JsonTextReader(sr);
                var diff = serializer.Deserialize<DifficultyV2>(jsonTextReader);
                return DifficultyV3.V2toV3(diff, bpm);
            }
            catch
            {
                return null;
            }
        }*/

        internal static bool IsBetweenVersions(string minVersion, string maxVersion, string version)
        {
            // Parse versions
            var minComponents = minVersion.Split('.').Select(int.Parse).ToArray();
            var maxComponents = maxVersion.Split('.').Select(int.Parse).ToArray();
            var versionComponents = version.Split('.').Select(int.Parse).ToArray();

            // Check if the version is above the minimum version
            if (versionComponents[0] > minComponents[0] ||
                (versionComponents[0] == minComponents[0] && versionComponents[1] > minComponents[1]) ||
                (versionComponents[0] == minComponents[0] && versionComponents[1] == minComponents[1] && versionComponents[2] >= minComponents[2]))
            {
                // Check if the version is below the maximum version
                if (versionComponents[0] < maxComponents[0] ||
                    (versionComponents[0] == maxComponents[0] && versionComponents[1] < maxComponents[1]) ||
                    (versionComponents[0] == maxComponents[0] && versionComponents[1] == maxComponents[1] && versionComponents[2] <= maxComponents[2]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
