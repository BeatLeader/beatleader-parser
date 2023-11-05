namespace Parser.Map.Difficulty.V3.Event
{
    public class Customdata
    {
        public float[] color { get; set; }
        public float rotation { get; set; }
        public float prop { get; set; }
        public float speed { get; set; }
        public float step { get; set; }
        public int[] lightID { get; set; }
        public int direction { get; set; }
        public bool lockRotation { get; set; }
        public string lerpType { get; set; }
        public string easing { get; set; }
    }
}
