using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BeatMapParser.Map.Difficulty.V3.Event.V3
{
    public class BasicEventTypesWithKeywords
    {
        [JsonPropertyName("d")]
        public List<BasicEventTypesForKeyword> data { get; }

        public BasicEventTypesWithKeywords(List<BasicEventTypesForKeyword> data) {
            this.data = data;
        }

        public class BasicEventTypesForKeyword
        {
            [JsonPropertyName("k")]
            public string keyword { get; }
            [JsonPropertyName("e")]
            public List<int> eventTypes { get; }

            public BasicEventTypesForKeyword(string keyword, List<int> eventTypes) {
                this.keyword = keyword;
                this.eventTypes = eventTypes;
            }
        }
    }
}
