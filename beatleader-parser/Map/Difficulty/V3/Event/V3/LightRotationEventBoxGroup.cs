using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Event.V3
{
    public class Lightrotationeventboxgroup : BeatmapObject
    {
        public int g { get; set; }
        public List<E2> e { get; set; }
    }

    public class E2
    {
        public F f { get; set; }
        public float w { get; set; }
        public int d { get; set; }
        public float s { get; set; }
        public int t { get; set; }
        public int b { get; set; }
        public int a { get; set; }
        public int r { get; set; }
        public int i { get; set; }
        public List<L> l { get; set; }
    }

    public class L
    {
        public float b { get; set; }
        public float r { get; set; }
        public int o { get; set; }
        public int e { get; set; }
        public int l { get; set; }
        public int p { get; set; }
    }
}
