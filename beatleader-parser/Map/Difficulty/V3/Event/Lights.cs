using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class Lights : BeatmapObject
    {
        [JsonProperty(PropertyName = "et")]
        public int Type { get; set; }
        [JsonProperty(PropertyName = "i")]
        public int Value { get; set; }
        [JsonProperty(PropertyName = "f")]
        public float f { get; set; }
        public Customdata customData { get; set; }

        public bool isBlue => Value == 1 || Value == 2 || Value == 3 || Value == 4;
        public bool isRed => Value == 5 || Value == 6 || Value == 7 || Value == 8;
        public bool isWhite => Value == 9 || Value == 10 || Value == 11 || Value == 12;
        public bool isOff => Value == 0;
        public bool isOn => Value == 1 || Value == 5 || Value == 9;
        public bool isFlash => Value == 2 || Value == 6 || Value == 10;
        public bool isFade => Value == 3 || Value == 7 || Value == 11;
        public bool isTransition => Value == 4 || Value == 8 || Value == 12;
    }
}
