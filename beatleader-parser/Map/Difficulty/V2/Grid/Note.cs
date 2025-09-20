namespace BeatMapParser.Map.Difficulty.V2.Grid
{
    public class Note
    {
        public float _time { get; set; }
        public int _lineIndex { get; set; }
        public int _lineLayer { get; set; }
        public int _type { get; set; }
        public int _cutDirection { get; set; }
        public NoteCustomData? _customData { get; set; }
    }

    public class NoteCustomData
    {
        public float[]? _position { get; set; }
    }
}
