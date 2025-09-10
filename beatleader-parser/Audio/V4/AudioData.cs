using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Parser.Audio.V4 {
    public class AudioData {
        [JsonPropertyName("version")]
        public string version { get; set; }
        [JsonPropertyName("songChecksum")] 
        public string songChecksum { get; set; }
        [JsonPropertyName("songSampleCount")]
        public int songSampleCount { get; set; }
        [JsonPropertyName("songFrequency")]
        public int songFrequency { get; set; }
        [JsonPropertyName("bpmData")]
        public List<BpmData> bpmData { get; set; }
        [JsonPropertyName("lufsData")]
        public List<object> lufsData { get; set; }
    }

    public class BpmData {
        [JsonPropertyName("si")]
        public int startSampleIndex { get; set; }
        [JsonPropertyName("ei")]
        public int endSampleIndex { get; set; }
        [JsonPropertyName("sb")]
        public float startBeat { get; set; }
        [JsonPropertyName("eb")]
        public float endBeat { get; set; }
    }
}
