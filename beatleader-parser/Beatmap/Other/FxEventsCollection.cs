using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class FxEventsCollection
    {
        [JsonProperty(PropertyName = "_il")]
        public List<IntFxEvent> IntEvents { get; set; }
        [JsonProperty(PropertyName = "_fl")]
        public List<FloatFxEvent> FloatFxEvents { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FxEventsCollection collection &&
                   EqualityComparer<List<IntFxEvent>>.Default.Equals(IntEvents, collection.IntEvents) &&
                   EqualityComparer<List<FloatFxEvent>>.Default.Equals(FloatFxEvents, collection.FloatFxEvents);
        }

        public override int GetHashCode()
        {
            int hashCode = -1151604215;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<IntFxEvent>>.Default.GetHashCode(IntEvents);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<FloatFxEvent>>.Default.GetHashCode(FloatFxEvents);
            return hashCode;
        }
    }

    public class IntFxEvent : BeatmapObject
    {
        [JsonProperty(PropertyName = "p")]
        public bool UsePreviousEventValue { get; set; }
        [JsonProperty(PropertyName = "v")]
        public int Value { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IntFxEvent @event &&
                   base.Equals(obj) &&
                   Beat == @event.Beat &&
                   UsePreviousEventValue == @event.UsePreviousEventValue &&
                   Value == @event.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = -1340859275;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + UsePreviousEventValue.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }
    }

    public class FloatFxEvent : BeatmapObject
    {
        [JsonProperty(PropertyName = "p")]
        public bool UsePreviousEventValue { get; set; }
        [JsonProperty(PropertyName = "v")]
        public float Value { get; set; }
        [JsonProperty(PropertyName = "i")]
        public int EaseType { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FloatFxEvent @event &&
                   base.Equals(obj) &&
                   Beat == @event.Beat &&
                   UsePreviousEventValue == @event.UsePreviousEventValue &&
                   Value == @event.Value &&
                   EaseType == @event.EaseType;
        }

        public override int GetHashCode()
        {
            int hashCode = 1593458340;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + UsePreviousEventValue.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EaseType.GetHashCode();
            return hashCode;
        }
    }
}
