using Parser.Audio.V4;
using Parser.Map;
using Parser.Map.Difficulty.V2.Base;
using Parser.Map.Difficulty.V3.Base;
using Parser.Map.Difficulty.V4.Base;
using Parser.Map.V4;
using System.Text.Json.Serialization;

namespace beatleader_parser.Utils
{
    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
    [JsonSerializable(typeof(Info))]
    [JsonSerializable(typeof(DifficultyV2))]
    internal partial class SerializeV2Context : JsonSerializerContext
    {
    }

    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
    [JsonSerializable(typeof(Info))]
    [JsonSerializable(typeof(DifficultyV3))]
    internal partial class SerializeV3Context : JsonSerializerContext
    {
    }

    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
    [JsonSerializable(typeof(InfoV4))]
    [JsonSerializable(typeof(DifficultyV4))]
    [JsonSerializable(typeof(AudioData))]
    [JsonSerializable(typeof(Lighting))]
    internal partial class SerializeV4Context : JsonSerializerContext
    {
    }
}
