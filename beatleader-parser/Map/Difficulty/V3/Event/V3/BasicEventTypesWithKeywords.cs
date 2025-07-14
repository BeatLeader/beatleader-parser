using Newtonsoft.Json;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Event.V3
{
    public class BasicEventTypesWithKeywords
    {
        [JsonProperty(PropertyName = "d")]
        public List<BasicEventTypesForKeyword> data { get; }

        public BasicEventTypesWithKeywords(List<BasicEventTypesForKeyword> data) {
            this.data = data;
        }

        public class BasicEventTypesForKeyword
        {
            [JsonProperty(PropertyName = "k")]
            public string keyword { get; }
            [JsonProperty(PropertyName = "e")]
            public List<int> eventTypes { get; }

            public BasicEventTypesForKeyword(string keyword, List<int> eventTypes) {
                this.keyword = keyword;
                this.eventTypes = eventTypes;
            }
        }
    }
}
