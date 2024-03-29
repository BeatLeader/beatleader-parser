﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Event
{
    public class Customdata
    {
        public float[] color { get; set; }
        public float rotation { get; set; }
        public float prop { get; set; }
        public float speed { get; set; }
        public float step { get; set; }

        [JsonConverter(typeof(LightIDConverter))]
        public int[] lightID { get; set; }
        public int direction { get; set; }
        public bool lockRotation { get; set; }
        public string lerpType { get; set; }
        public string easing { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Customdata customdata &&
                   EqualityComparer<float[]>.Default.Equals(color, customdata.color) &&
                   rotation == customdata.rotation &&
                   prop == customdata.prop &&
                   speed == customdata.speed &&
                   step == customdata.step &&
                   EqualityComparer<int[]>.Default.Equals(lightID, customdata.lightID) &&
                   direction == customdata.direction &&
                   lockRotation == customdata.lockRotation &&
                   lerpType == customdata.lerpType &&
                   easing == customdata.easing;
        }

        public override int GetHashCode()
        {
            int hashCode = -396724278;
            hashCode = hashCode * -1521134295 + EqualityComparer<float[]>.Default.GetHashCode(color);
            hashCode = hashCode * -1521134295 + rotation.GetHashCode();
            hashCode = hashCode * -1521134295 + prop.GetHashCode();
            hashCode = hashCode * -1521134295 + speed.GetHashCode();
            hashCode = hashCode * -1521134295 + step.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(lightID);
            hashCode = hashCode * -1521134295 + direction.GetHashCode();
            hashCode = hashCode * -1521134295 + lockRotation.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(lerpType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(easing);
            return hashCode;
        }
    }

    public class LightIDConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<int[]>();
            }
            return new int[] { token.ToObject<int>() };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            int[] lightIDs = (int[])value;
            if (lightIDs.Length == 1)
            {
                serializer.Serialize(writer, lightIDs[0]);
            }
            else
            {
                serializer.Serialize(writer, lightIDs);
            }
        }
    }
}
