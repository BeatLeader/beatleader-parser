using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class Waypoint : GridObject
    {
        [JsonProperty(PropertyName = "d")]
        public ObjectDirection OffsetDirection { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Waypoint waypoint &&
                   base.Equals(obj) &&
                   Beat == waypoint.Beat &&
                   X == waypoint.X &&
                   Y == waypoint.Y &&
                   OffsetDirection == waypoint.OffsetDirection;
        }

        public override int GetHashCode()
        {
            int hashCode = -71264547;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + OffsetDirection.GetHashCode();
            return hashCode;
        }
    }
}
