using Parser.Map.Difficulty.V3.Base;

namespace Parser.Map.Difficulty.V3.Event
{
    public class Basicbeatmapevent : BeatmapObject
    {
        public int et { get; set; }
        public int i { get; set; }
        public float f { get; set; }
        public Customdata customData { get; set; }

        public bool IsBlue => et == 1 || et == 2 || et == 3 || et == 4;
        public bool IsRed => et == 5 || et == 6 || et == 7 || et == 8;
        public bool IsWhite => et == 9 || et == 10 || et == 11 || et == 12;
        public bool IsOff => et == 0;
        public bool IsOn => et == 1 || et == 5 || et == 9;
        public bool IsFlash => et == 2 || et == 6 || et == 10;
        public bool IsFade => et == 3 || et == 7 || et == 11;
        public bool IsTransition => et == 4 || et == 8 || et == 12;
    }
}
