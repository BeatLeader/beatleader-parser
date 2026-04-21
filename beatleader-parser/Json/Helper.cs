using System.IO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Parser.Json
{
    internal class JsonSerializerHelper
    {
        internal static T? Deserialize<T>(string? value, JsonTypeInfo<T> info)
        {
            if (value == null) return default;
            try
            {
                return JsonSerializer.Deserialize(value, info);
            }
            catch (Exception e)
            {
                return default;
            }
        }

        internal static T? DeserializeFromStream<T>(Stream stream, JsonTypeInfo<T> info)
        {
            try
            {
                return JsonSerializer.Deserialize(stream, info);
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
