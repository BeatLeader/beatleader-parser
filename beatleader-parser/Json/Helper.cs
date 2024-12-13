using Parser.Map.Difficulty.V2.Base;
using Parser.Map.Difficulty.V3.Base;
using Parser.Map;
using Newtonsoft.Json;
using System.IO;
using System;
using Parser.Audio.V4;

namespace Parser.Json
{
    internal class Helper
    {
        internal static T? DeserializeFromStream<T>(Stream stream)
        {
            try
            {
                var serializer = new JsonSerializer();
                using var sr = new StreamReader(stream);
                using var jsonTextReader = new JsonTextReader(sr);
                return serializer.Deserialize<T>(jsonTextReader);
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
