using Parser.Map.Difficulty.V2.Base;
using Parser.Map.Difficulty.V3.Base;
using Parser.Map;
using Newtonsoft.Json;
using System.IO;

namespace Parser.Json
{
    internal class OldHelper
    {
        internal static Info DeserializeInfoFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();
            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);
            return serializer.Deserialize<Info>(jsonTextReader);
        }

        internal static DifficultyV3 DeserializeV3DiffFromStream(Stream stream)
        {
            try
            {
                var serializer = new JsonSerializer();
                using var sr = new StreamReader(stream);
                using var jsonTextReader = new JsonTextReader(sr);
                return serializer.Deserialize<DifficultyV3>(jsonTextReader);
            }
            catch 
            {
                return null;
            }
        }

        internal static DifficultyV3 DeserializeV2DiffFromStream(Stream stream, float bpm)
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
        }
    }
}
