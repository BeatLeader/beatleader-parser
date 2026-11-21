using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V3.Base
{
    public class GridObjectCustomData
    {
        public float[]? coordinates { get; set; }
        public float[]? tailCoordinates { get; set; }
    }

    // Reads the coordinates of a V3 object's customData, anything else or any unexpected shape is ignored so a map still loads
    public class GridObjectCustomDataConverter : JsonConverter<GridObjectCustomData?>
    {
        public override GridObjectCustomData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object) return null;
            var data = new GridObjectCustomData
            {
                coordinates = Numbers(root, "coordinates"),
                tailCoordinates = Numbers(root, "tailCoordinates")
            };
            return data.coordinates == null && data.tailCoordinates == null ? null : data;
        }

        private static float[]? Numbers(JsonElement root, string name)
        {
            if (!root.TryGetProperty(name, out var array) || array.ValueKind != JsonValueKind.Array) return null;
            var values = new List<float>();
            foreach (var item in array.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Number) return null;
                values.Add(item.GetSingle());
            }
            return values.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, GridObjectCustomData? value, JsonSerializerOptions options)
        {
            if (value == null) { writer.WriteNullValue(); return; }
            writer.WriteStartObject();
            if (value.coordinates != null) { writer.WritePropertyName("coordinates"); Write(writer, value.coordinates); }
            if (value.tailCoordinates != null) { writer.WritePropertyName("tailCoordinates"); Write(writer, value.tailCoordinates); }
            writer.WriteEndObject();
        }

        private static void Write(Utf8JsonWriter writer, float[] values)
        {
            writer.WriteStartArray();
            foreach (var v in values) writer.WriteNumberValue(v);
            writer.WriteEndArray();
        }
    }
}
