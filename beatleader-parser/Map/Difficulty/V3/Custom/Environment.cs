namespace Parser.Map.Difficulty.V3.Custom
{
    public class Environment
    {
        public string id { get; set; }
        public string lookupMethod { get; set; }
        public Components components { get; set; }
        public bool active { get; set; }
        public float[] position { get; set; }
        public float[] scale { get; set; }
        public float[] rotation { get; set; }
        public Geometry geometry { get; set; }
        public int duplicate { get; set; }
    }
}
