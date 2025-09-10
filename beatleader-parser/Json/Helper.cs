using System.IO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Parser.Json
{
    internal class Helper
    {
        internal static T? DeserializeFromStream<T>(Stream stream, JsonTypeInfo<T> info)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(stream, info);
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
