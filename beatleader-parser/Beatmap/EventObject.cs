using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class EventObject : BeatmapObject
    {
        public override bool Equals(object obj)
        {
            return obj is EventObject @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat;
        }

        public override int GetHashCode()
        {
            int hashCode = 650526578;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            return hashCode;
        }
    }
}
