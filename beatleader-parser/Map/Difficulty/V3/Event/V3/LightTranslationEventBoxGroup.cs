using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Event.V3
{
    public class Lighttranslationeventboxgroup : BeatmapObject
    {
        public int g { get; set; }
        public List<E3> e { get; set; }
    }

    public class E3
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
        public List<L1> l { get; set; }
    }

    public class L1
    {
        public float b { get; set; }
        public int p { get; set; }
        public int e { get; set; }
        public float t { get; set; }
    }
}
