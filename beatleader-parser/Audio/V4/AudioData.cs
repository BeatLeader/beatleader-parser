using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Parser.Audio.V4 {
    public class AudioData {
        [JsonProperty("version")]
        public string version { get; set; }
        [JsonProperty("songChecksum")] 
        public string songChecksum { get; set; }
        [JsonProperty("songSampleCount")]
        public int songSampleCount { get; set; }
        [JsonProperty("songFrequency")]
        public int songFrequency { get; set; }
        [JsonProperty("bpmData")]
        public List<BpmData> bpmData { get; set; }
        [JsonProperty("lufsData")]
        public List<object> lufsData { get; set; }
    }

    public class BpmData {
        [JsonProperty("si")]
        public int startSampleIndex { get; set; }
        [JsonProperty("ei")]
        public int endSampleIndex { get; set; }
        [JsonProperty("sb")]
        public float startBeat { get; set; }
        [JsonProperty("eb")]
        public float endBeat { get; set; }
    }
}
