using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Event.V3
{
    public class Lightcoloreventboxgroup : BeatmapObject
    {
        public int g { get; set; }
        public List<E> e { get; set; }
    }

    public class E
    {
        public F f { get; set; }
        public float w { get; set; }
        public int d { get; set; }
        public float r { get; set; }
        public int t { get; set; }
        public int b { get; set; }
        public int i { get; set; }
        public List<E1> e { get; set; }
    }

    public class F
    {
        public int f { get; set; }
        public int p { get; set; }
        public int t { get; set; }
        public int r { get; set; }
        public int c { get; set; }
        public int n { get; set; }
        public int s { get; set; }
        public float l { get; set; }
        public int d { get; set; }
    }

    public class E1
    {
        public float b { get; set; }
        public int c { get; set; }
        public float s { get; set; }
        public int i { get; set; }
        public int f { get; set; }
    }
}
