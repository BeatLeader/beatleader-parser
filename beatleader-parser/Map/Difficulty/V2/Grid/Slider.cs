namespace Parser.Map.Difficulty.V2.Grid
{
    public class Slider
    {
        public int colorType { get; set; }
        public float _headTime { get; set; }
        public int _headLineIndex { get; set; }
        public int _headLineLayer { get; set; }
        public float _headControlPointLengthMultiplier { get; set; }
        public int _headCutDirection { get; set; }
        public float _tailTime { get; set; }
        public int _tailLineIndex { get; set; }
        public int _tailLineLayer { get; set; }
        public float _tailControlPointLengthMultiplier { get; set; }
        public int _tailCutDirection { get; set; }
        public int _sliderMidAnchorMode { get; set; }
    }
}
