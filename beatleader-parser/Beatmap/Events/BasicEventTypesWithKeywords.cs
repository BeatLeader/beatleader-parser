using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class BasicEventTypesWithKeywords
    {
        [JsonProperty(PropertyName = "d")]
        public List<BasicEventTypesForKeyword> Keywords { get; set; }
    }

    public class BasicEventTypesForKeyword
    {
        [JsonProperty(PropertyName = "k")]
        public string Keyword { get; set; }
        [JsonProperty(PropertyName = "e")]
        public List<BasicEvent> Events { get; set; }

    }
}
