using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parser.Map.Difficulty.V3.Event.V3
{
    public class Lightcoloreventboxgroup : BeatmapObject
    {
        [JsonProperty(PropertyName = "g")]
        public int Group { get; set; }
        [JsonProperty(PropertyName = "e")]
        public List<E> EventBoxGroup { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Lightcoloreventboxgroup lightcoloreventboxgroup &&
                   base.Equals(obj) &&
                   Beats == lightcoloreventboxgroup.Beats &&
                   Group == lightcoloreventboxgroup.Group &&
                   EqualityComparer<List<E>>.Default.Equals(EventBoxGroup, lightcoloreventboxgroup.EventBoxGroup);
        }

        public override int GetHashCode()
        {
            int hashCode = 893042503;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + Group.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<E>>.Default.GetHashCode(EventBoxGroup);
            return hashCode;
        }
    }

    public class E
    {
        public F Filter { get; set; }
        public float w { get; set; }
        public int d { get; set; }
        public float r { get; set; }
        public int t { get; set; }
        public int b { get; set; }
        public int i { get; set; }
        public List<E1> e { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            E otherEvent = (E)obj;
            return Equals(Filter, otherEvent.Filter) &&
                   Equals(w, otherEvent.w) &&
                   Equals(d, otherEvent.d) &&
                   Equals(r, otherEvent.r) &&
                   Equals(t, otherEvent.t) &&
                   Equals(b, otherEvent.b) &&
                   Equals(i, otherEvent.i) &&
                   Equals(e, otherEvent.e);
        }
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            F otherEvent = (F)obj;
            return Equals(f, otherEvent.f) &&
                   Equals(p, otherEvent.p) &&
                   Equals(t, otherEvent.t) &&
                   Equals(r, otherEvent.r) &&
                   Equals(c, otherEvent.c) &&
                   Equals(n, otherEvent.n) &&
                   Equals(s, otherEvent.s) &&
                   Equals(l, otherEvent.l) &&
                   Equals(d, otherEvent.d);
        }
    }

    public class E1
    {
        public float b { get; set; }
        public int c { get; set; }
        public float s { get; set; }
        public int i { get; set; }
        public int f { get; set; }
        public float sb { get; set; }
        public int sf { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            E1 otherEvent = (E1)obj;
            return Equals(b, otherEvent.b) &&
                   Equals(c, otherEvent.c) &&
                   Equals(s, otherEvent.s) &&
                   Equals(i, otherEvent.i) &&
                   Equals(f, otherEvent.f);
        }
    }
}
