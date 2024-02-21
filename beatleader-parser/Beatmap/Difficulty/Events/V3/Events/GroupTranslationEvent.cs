using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class GroupTranslationEvent : GroupMovementEvent
    {
        [JsonProperty(PropertyName = "t")]
        public float Translation { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupTranslationEvent @event &&
                   base.Equals(obj) &&
                   Beat == @event.Beat &&
                   ExtendsRotation == @event.ExtendsRotation &&
                   EaseType == @event.EaseType &&
                   Translation == @event.Translation;
        }

        public override int GetHashCode()
        {
            int hashCode = 1420696354;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beat.GetHashCode();
            hashCode = hashCode * -1521134295 + ExtendsRotation.GetHashCode();
            hashCode = hashCode * -1521134295 + EaseType.GetHashCode();
            hashCode = hashCode * -1521134295 + Translation.GetHashCode();
            return hashCode;
        }
    }
}
