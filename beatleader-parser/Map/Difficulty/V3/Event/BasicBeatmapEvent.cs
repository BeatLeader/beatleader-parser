using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class Basicbeatmapevent : BeatmapObject
    {
        public int et { get; set; }
        public int i { get; set; }
        public float f { get; set; }
        public Customdata customData { get; set; }

        public bool IsBlue => i == 1 || i == 2 || i == 3 || i == 4;
        public bool IsRed => i == 5 || i == 6 || i == 7 || i == 8;
        public bool IsWhite => i == 9 || i == 10 || i == 11 || i == 12;
        public bool IsOff => i == 0;
        public bool IsOn => i == 1 || i == 5 || i == 9;
        public bool IsFlash => i == 2 || i == 6 || i == 10;
        public bool IsFade => i == 3 || i == 7 || i == 11;
        public bool IsTransition => i == 4 || i == 8 || i == 12;
    }
}
