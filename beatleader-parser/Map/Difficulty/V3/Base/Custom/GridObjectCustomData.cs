using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V3.Base
{
    public class GridObjectCustomData
    {
        public float[]? coordinates { get; set; }
        public float[]? tailCoordinates { get; set; }
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? Extra { get; set; }
    }
}
