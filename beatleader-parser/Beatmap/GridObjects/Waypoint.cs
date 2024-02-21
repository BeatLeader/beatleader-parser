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
    }
}
