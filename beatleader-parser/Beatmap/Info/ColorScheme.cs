using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class ColorSchemeEntry
    {
        [JsonProperty(PropertyName = "useOverride")]
        public bool UseOverride { get; set; }
        [JsonProperty(PropertyName = "colorScheme")]
        public ColorScheme ColorScheme { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ColorSchemeEntry entry &&
                   UseOverride == entry.UseOverride &&
                   EqualityComparer<ColorScheme>.Default.Equals(ColorScheme, entry.ColorScheme);
        }

        public override int GetHashCode()
        {
            int hashCode = -1928628549;
            hashCode = hashCode * -1521134295 + UseOverride.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorScheme>.Default.GetHashCode(ColorScheme);
            return hashCode;
        }
    }

    public class ColorScheme
    {
        [JsonProperty(PropertyName = "colorSchemeId")]
        public string ColorSchemeId { get; set; }
        [JsonProperty(PropertyName = "SaberAColor")]
        public ColorPalette SaberAColor { get; set; }
        [JsonProperty(PropertyName = "SaberBColor")]
        public ColorPalette SaberBColor { get; set; }
        [JsonProperty(PropertyName = "obstacleColor")]
        public ColorPalette ObstacleColor { get; set; }
        [JsonProperty(PropertyName = "environmentColor0")]
        public ColorPalette EnvironmentColor0 { get; set; }
        [JsonProperty(PropertyName = "environmentColor1")]
        public ColorPalette EnvironmentColor1 { get; set; }
        [JsonProperty(PropertyName = "environmentColor0Boost")]
        public ColorPalette EnvironmentColor0Boost { get; set; }
        [JsonProperty(PropertyName = "environmentColor1Boost")]
        public ColorPalette EnvironmentColor1Boost { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ColorScheme scheme &&
                   ColorSchemeId == scheme.ColorSchemeId &&
                   EqualityComparer<ColorPalette>.Default.Equals(SaberAColor, scheme.SaberAColor) &&
                   EqualityComparer<ColorPalette>.Default.Equals(SaberBColor, scheme.SaberBColor) &&
                   EqualityComparer<ColorPalette>.Default.Equals(ObstacleColor, scheme.ObstacleColor) &&
                   EqualityComparer<ColorPalette>.Default.Equals(EnvironmentColor0, scheme.EnvironmentColor0) &&
                   EqualityComparer<ColorPalette>.Default.Equals(EnvironmentColor1, scheme.EnvironmentColor1) &&
                   EqualityComparer<ColorPalette>.Default.Equals(EnvironmentColor0Boost, scheme.EnvironmentColor0Boost) &&
                   EqualityComparer<ColorPalette>.Default.Equals(EnvironmentColor1Boost, scheme.EnvironmentColor1Boost);
        }

        public override int GetHashCode()
        {
            int hashCode = -423334252;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ColorSchemeId);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorPalette>.Default.GetHashCode(SaberAColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorPalette>.Default.GetHashCode(SaberBColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorPalette>.Default.GetHashCode(ObstacleColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorPalette>.Default.GetHashCode(EnvironmentColor0);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorPalette>.Default.GetHashCode(EnvironmentColor1);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorPalette>.Default.GetHashCode(EnvironmentColor0Boost);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorPalette>.Default.GetHashCode(EnvironmentColor1Boost);
            return hashCode;
        }
    }

    public class ColorPalette
    {
        [JsonProperty(PropertyName = "r")]
        public float R { get; set; }
        [JsonProperty(PropertyName = "g")]
        public float G { get; set; }
        [JsonProperty(PropertyName = "b")]
        public float B { get; set; }
        [JsonProperty(PropertyName = "a")]
        public float A { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ColorPalette palette &&
                   R == palette.R &&
                   G == palette.G &&
                   B == palette.B &&
                   A == palette.A;
        }

        public override int GetHashCode()
        {
            int hashCode = 1960784236;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            return hashCode;
        }
    }
}
