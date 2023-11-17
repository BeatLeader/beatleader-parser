using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Event.V3
{
    public class Lightrotationeventboxgroup : BeatmapObject
    {
        [JsonProperty(PropertyName = "g")]
        public int Group { get; set; }
        [JsonProperty(PropertyName = "e")]
        public List<E2> EventBoxGroup { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Lightrotationeventboxgroup otherEvent = (Lightrotationeventboxgroup)obj;
            return Equals(Beats, otherEvent.Beats) &&
                   Equals(Group, otherEvent.Group) &&
                   Equals(EventBoxGroup, otherEvent.EventBoxGroup);
        }
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            E2 otherEvent = (E2)obj;
            return Equals(f, otherEvent.f) &&
                   Equals(w, otherEvent.w) &&
                   Equals(d, otherEvent.d) &&
                   Equals(s, otherEvent.s) &&
                   Equals(t, otherEvent.t) &&
                   Equals(b, otherEvent.b) &&
                   Equals(a, otherEvent.a) &&
                   Equals(r, otherEvent.r) &&
                   Equals(i, otherEvent.i) &&
                   Equals(l, otherEvent.l);
        }
    }

    public class L
    {
        public float b { get; set; }
        public float r { get; set; }
        public int o { get; set; }
        public int e { get; set; }
        public int l { get; set; }
        public int p { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            L otherEvent = (L)obj;
            return Equals(b, otherEvent.b) &&
                   Equals(r, otherEvent.r) &&
                   Equals(o, otherEvent.o) &&
                   Equals(e, otherEvent.e) &&
                   Equals(l, otherEvent.l) &&
                   Equals(p, otherEvent.p);
        }
    }
}
