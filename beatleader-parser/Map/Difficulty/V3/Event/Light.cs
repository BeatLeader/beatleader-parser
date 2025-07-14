using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Event
{
    public class Light : BeatmapObject
    {
        [JsonProperty(PropertyName = "et")]
        public int Type { get; set; }
        [JsonProperty(PropertyName = "i")]
        public int Value { get; set; }
        [JsonProperty(PropertyName = "f")]
        public float f { get; set; }

        [JsonIgnore]
        public bool isBlue => Value == 1 || Value == 2 || Value == 3 || Value == 4;
        [JsonIgnore]
        public bool isRed => Value == 5 || Value == 6 || Value == 7 || Value == 8;
        [JsonIgnore]
        public bool isWhite => Value == 9 || Value == 10 || Value == 11 || Value == 12;
        [JsonIgnore]
        public bool isOff => Value == 0;
        [JsonIgnore]
        public bool isOn => Value == 1 || Value == 5 || Value == 9;
        [JsonIgnore]
        public bool isFlash => Value == 2 || Value == 6 || Value == 10;
        [JsonIgnore]
        public bool isFade => Value == 3 || Value == 7 || Value == 11;
        [JsonIgnore]
        public bool isTransition => Value == 4 || Value == 8 || Value == 12;

        public override bool Equals(object obj)
        {
            return obj is Light light &&
                   Beats == light.Beats &&
                   Type == light.Type &&
                   Value == light.Value &&
                   f == light.f;
        }

        public override int GetHashCode()
        {
            int hashCode = 1853604182;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + f.GetHashCode();
            return hashCode;
        }
    }
}
