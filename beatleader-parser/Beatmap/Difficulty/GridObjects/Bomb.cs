using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class Bomb : GridObject
    {
        // This type of object does not have any additional properties

        public override bool Equals(object obj)
        {
            return obj is Bomb @object &&
                   base.Equals(obj) &&
                   Beat == @object.Beat &&
                   X == @object.X &&
                   Y == @object.Y;
        }

        public override int GetHashCode()
        {
            int hashCode = 23480321;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
