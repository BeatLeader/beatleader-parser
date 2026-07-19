using System;
using System.IO;
using System.Text;
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

        internal static string Serialize<T>(T value, JsonTypeInfo<T> info, bool writeIndented = false)
        {
            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = writeIndented }))
            {
                JsonSerializer.Serialize(writer, value, info);
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
