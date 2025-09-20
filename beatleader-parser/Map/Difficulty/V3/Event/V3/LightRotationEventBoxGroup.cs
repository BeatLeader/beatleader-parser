using BeatMapParser.Map.Difficulty.V3.Base;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BeatMapParser.Map.Difficulty.V3.Event.V3
{
    public class Lightrotationeventboxgroup : BeatmapObject
    {
        [JsonPropertyName("g")]
        public int Group { get; set; }
        [JsonPropertyName("e")]
        public List<E2> EventBoxGroup { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Lightrotationeventboxgroup lightrotationeventboxgroup &&
                   base.Equals(obj) &&
                   Beats == lightrotationeventboxgroup.Beats &&
                   Group == lightrotationeventboxgroup.Group &&
                   EqualityComparer<List<E2>>.Default.Equals(EventBoxGroup, lightrotationeventboxgroup.EventBoxGroup);
        }

        public override int GetHashCode()
        {
            int hashCode = 893042503;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + Group.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<E2>>.Default.GetHashCode(EventBoxGroup);
            return hashCode;
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
