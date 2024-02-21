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

        public override bool Equals(object obj)
        {
            return obj is BasicEventTypesWithKeywords keywords &&
                   EqualityComparer<List<BasicEventTypesForKeyword>>.Default.Equals(Keywords, keywords.Keywords);
        }

        public override int GetHashCode()
        {
            return -1062200643 + EqualityComparer<List<BasicEventTypesForKeyword>>.Default.GetHashCode(Keywords);
        }
    }

    public class BasicEventTypesForKeyword
    {
        [JsonProperty(PropertyName = "k")]
        public string Keyword { get; set; }
        [JsonProperty(PropertyName = "e")]
        public List<int> Events { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BasicEventTypesForKeyword keyword &&
                   Keyword == keyword.Keyword &&
                   EqualityComparer<List<int>>.Default.Equals(Events, keyword.Events);
        }

        public override int GetHashCode()
        {
            int hashCode = -300249624;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Keyword);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(Events);
            return hashCode;
        }
    }
}
