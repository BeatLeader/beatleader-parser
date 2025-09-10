using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V3.Event.V3
{
    public class Lighttranslationeventboxgroup : BeatmapObject
    {
        [JsonPropertyName("g")]
        public int Group { get; set; }
        [JsonPropertyName("e")]
        public List<E3> EventBoxGroup { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Lighttranslationeventboxgroup lighttranslationeventboxgroup &&
                   base.Equals(obj) &&
                   Beats == lighttranslationeventboxgroup.Beats &&
                   Group == lighttranslationeventboxgroup.Group &&
                   EqualityComparer<List<E3>>.Default.Equals(EventBoxGroup, lighttranslationeventboxgroup.EventBoxGroup);
        }

        public override int GetHashCode()
        {
            int hashCode = 893042503;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + Group.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<E3>>.Default.GetHashCode(EventBoxGroup);
            return hashCode;
        }
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

        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            E3 otherEvent = (E3)obj;
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

    public class L1
    {
        public float b { get; set; }
        public int p { get; set; }
        public int e { get; set; }
        public float t { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            L1 otherEvent = (L1)obj;
            return Equals(b, otherEvent.b) &&
                   Equals(p, otherEvent.p) &&
                   Equals(e, otherEvent.e) &&
                   Equals(e, otherEvent.e) &&
                   Equals(t, otherEvent.t);
        }
    }
}
