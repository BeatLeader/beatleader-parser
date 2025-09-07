using beatleader_parser.Timescale;
using Newtonsoft.Json;
using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V4.Base
{
    public class DifficultyV4
    {
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; } = "4.1.0";

        [JsonProperty(PropertyName = "colorNotes")]
        public List<BaseNote> colorNotes { get; set; } = new();
        [JsonProperty(PropertyName = "colorNotesData")] 
        public List<ColorNoteData> colorNotesData { get; set; } = new();
        [JsonProperty(PropertyName = "bombNotes")]
        public List<BaseNote> bombNotes { get; set; } = new();
        [JsonProperty(PropertyName = "bombNotesData")]
        public List<BombNoteData> bombNotesData { get; set; } = new();
        [JsonProperty(PropertyName = "obstacles")]
        public List<BaseNote> obstacles { get; set; } = new();
        [JsonProperty(PropertyName = "obstaclesData")]
        public List<ObstacleData> obstaclesData { get; set; } = new();
        [JsonProperty(PropertyName = "arcs")]
        public List<ArcNote> arcs { get; set; } = new();
        [JsonProperty(PropertyName = "arcsData")]
        public List<ArcData> arcsData { get; set; } = new();
        [JsonProperty(PropertyName = "chains")]
        public List<ChainNote> chains { get; set; } = new();
        [JsonProperty(PropertyName = "chainsData")]
        public List<ChainData> chainsData { get; set; } = new();
        [JsonProperty(PropertyName = "spawnRotations")]
        public List<BaseEvent> spawnRotations { get; set; } = new();
        [JsonProperty(PropertyName = "spawnRotationsData")]
        public List<RotationData> spawnRotationsData { get; set; } = new();
        [JsonProperty(PropertyName = "njsEvents")]
        public List<BaseEvent> njsEvents { get; set; } = new();
        [JsonProperty(PropertyName = "njsEventData")]
        public List<NjsEventData> njsEventData { get; set; } = new();
        [JsonProperty(PropertyName = "waypoints")]
        public object[] Waypoints { get; set; }
    }

    public class Lighting {
        // V4 Lighting Box Groups
        [JsonProperty(PropertyName = "eventBoxGroups")]
        public List<EventBoxGroup> eventBoxGroups { get; set; } = new();
        [JsonProperty(PropertyName = "indexFilters")]
        public List<IndexFilter> indexFilters { get; set; } = new();
        [JsonProperty(PropertyName = "lightColorEventBoxes")]
        public List<LightColorEventBox> lightColorEventBoxes { get; set; } = new();
        [JsonProperty(PropertyName = "lightRotationEventBoxes")]
        public List<LightRotationEventBox> lightRotationEventBoxes { get; set; } = new();
        [JsonProperty(PropertyName = "lightTranslationEventBoxes")]
        public List<LightTranslationEventBox> lightTranslationEventBoxes { get; set; } = new();
        [JsonProperty(PropertyName = "lightColorEvents")]
        public List<LightColorEvent> lightColorEvents { get; set; } = new();
        [JsonProperty(PropertyName = "lightRotationEvents")]
        public List<LightRotationEvent> lightRotationEvents { get; set; } = new();
        [JsonProperty(PropertyName = "lightTranslationEvents")]
        public List<LightTranslationEvent> lightTranslationEvents { get; set; } = new();
    }

    public class BaseNote
    {
        [JsonProperty("b")]
        public float Beat { get; set; }
        [JsonProperty("r")]
        public int RotationLane { get; set; }
        [JsonProperty("i")]
        public int Index { get; set; }
    }

    public class ColorNoteData 
    {
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("c")]
        public int Color { get; set; }
        [JsonProperty("d")]
        public int Direction { get; set; }
        [JsonProperty("a")]
        public int AngleOffset { get; set; }
        public GridObjectCustomData? customData { get; set; }
    }

    public class BombNoteData
    {
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        public GridObjectCustomData? customData { get; set; }
    }

    public class ObstacleData
    {
        [JsonProperty("d")]
        public float Duration { get; set; }
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("w")]
        public int Width { get; set; }
        [JsonProperty("h")]
        public int Height { get; set; }
    }

    public class ArcNote
    {
        [JsonProperty("hb")]
        public float HeadBeat { get; set; }
        [JsonProperty("tb")]
        public float TailBeat { get; set; }
        [JsonProperty("hr")]
        public int HeadRow { get; set; }
        [JsonProperty("tr")]
        public int TailRow { get; set; }
        [JsonProperty("hi")]
        public int HeadIndex { get; set; }
        [JsonProperty("ti")]
        public int TailIndex { get; set; }
        [JsonProperty("ai")]
        public int ArcIndex { get; set; }
    }

    public class ArcData
    {
        [JsonProperty("m")]
        public float HeadControlPointLengthMultiplier { get; set; }
        [JsonProperty("tm")]
        public float TailControlPointLengthMultiplier { get; set; }
        [JsonProperty("a")]
        public int MidAnchorMode { get; set; }
        public GridObjectCustomData? customData { get; set; }
    }

    public class ChainNote
    {
        [JsonProperty("hb")]
        public float HeadBeat { get; set; }
        [JsonProperty("tb")]
        public float TailBeat { get; set; }
        [JsonProperty("hr")]
        public int HeadRow { get; set; }
        [JsonProperty("tr")]
        public int TailRow { get; set; }
        [JsonProperty("i")]
        public int Index { get; set; }
        [JsonProperty("ci")]
        public int ChainIndex { get; set; }
    }

    public class ChainData
    {
        [JsonProperty("tx")]
        public int TailX { get; set; }
        [JsonProperty("ty")]
        public int TailY { get; set; }
        [JsonProperty("c")]
        public int SliceCount { get; set; }
        [JsonProperty("s")]
        public float SquishFactor { get; set; }
        public GridObjectCustomData? customData { get; set; }
    }

    public class BaseEvent
    {
        [JsonProperty("b")]
        public float Beat { get; set; }
        [JsonProperty("i")]
        public int Index { get; set; }
    }

    public class RotationData
    {
        [JsonProperty("t")]
        public int Type { get; set; }
        [JsonProperty("r")]
        public float Rotation { get; set; }
    }

    public class NjsEventData {
        [JsonProperty(PropertyName = "d")]
        public float Delta { get; set; }
        [JsonProperty(PropertyName = "p")]
        public int UsePrevious { get; set; }
        [JsonProperty(PropertyName = "e")]
        public int Easing { get; set; }
    }

    // V4 Event Box Group Classes
    public class EventBoxGroup
    {
        [JsonProperty("b")]
        public float Beat { get; set; }
        [JsonProperty("g")]
        public int Group { get; set; }
        [JsonProperty("t")]
        public int Type { get; set; }
        [JsonProperty("e")]
        public List<EventBox> Events { get; set; } = new();
    }

    public class EventBox
    {
        [JsonProperty("f")]
        public int FilterIndex { get; set; }
        [JsonProperty("e")]
        public int EventBoxIndex { get; set; }
        [JsonProperty("l")]
        public List<BaseEvent> Events { get; set; } = new();
    }

    public class IndexFilter
    {
        [JsonProperty("c")]
        public int Chunks { get; set; }
        [JsonProperty("f")]
        public int Type { get; set; }
        [JsonProperty("p")]
        public int Parameter0 { get; set; }
        [JsonProperty("t")]
        public int Parameter1 { get; set; }
        [JsonProperty("r")]
        public int Reverse { get; set; }
        [JsonProperty("n")]
        public int RandomBehavior { get; set; }
        [JsonProperty("s")]
        public int RandomSeed { get; set; }
        [JsonProperty("l")]
        public float LimitPercent { get; set; }
        [JsonProperty("d")]
        public int LimitBehavior { get; set; }
    }

    public class LightColorEventBox
    {
        [JsonProperty("w")]
        public float BeatDistributionValue { get; set; }
        [JsonProperty("d")]
        public int BeatDistributionType { get; set; }
        [JsonProperty("s")]
        public float BrightnessDistributionValue { get; set; }
        [JsonProperty("t")]
        public int BrightnessDistributionType { get; set; }
        [JsonProperty("b")]
        public int BrightnessDistributionAffectsFirst { get; set; }
        [JsonProperty("e")]
        public int BrightnessDistributionEasing { get; set; }
    }

    public class LightRotationEventBox
    {
        [JsonProperty("w")]
        public float BeatDistributionValue { get; set; }
        [JsonProperty("d")]
        public int BeatDistributionType { get; set; }
        [JsonProperty("s")]
        public float RotationDistributionValue { get; set; }
        [JsonProperty("t")]
        public int RotationDistributionType { get; set; }
        [JsonProperty("b")]
        public int RotationDistributionAffectsFirst { get; set; }
        [JsonProperty("e")]
        public int RotationDistributionEasing { get; set; }
        [JsonProperty("a")]
        public int Axis { get; set; }
        [JsonProperty("f")]
        public int InvertAxis { get; set; }
    }

    public class LightTranslationEventBox
    {
        [JsonProperty("w")]
        public float BeatDistributionValue { get; set; }
        [JsonProperty("d")]
        public int BeatDistributionType { get; set; }
        [JsonProperty("s")]
        public float GapDistributionValue { get; set; }
        [JsonProperty("t")]
        public int GapDistributionType { get; set; }
        [JsonProperty("b")]
        public int GapDistributionAffectsFirst { get; set; }
        [JsonProperty("e")]
        public int GapDistributionEasing { get; set; }
        [JsonProperty("a")]
        public int Axis { get; set; }
        [JsonProperty("f")]
        public int InvertAxis { get; set; }
    }

    public class LightColorEvent
    {
        [JsonProperty("p")]
        public int TransitionType { get; set; }
        [JsonProperty("e")]
        public int Easing { get; set; }
        [JsonProperty("c")]
        public int Color { get; set; }
        [JsonProperty("b")]
        public float Brightness { get; set; }
        [JsonProperty("f")]
        public int StrobeFrequency { get; set; }
        [JsonProperty("sb")]
        public float StrobeBrightness { get; set; }
        [JsonProperty("sf")]
        public int StrobeFade { get; set; }
    }

    public class LightRotationEvent
    {
        [JsonProperty("p")]
        public int TransitionType { get; set; }
        [JsonProperty("e")]
        public int Easing { get; set; }
        [JsonProperty("r")]
        public float Magnitude { get; set; }
        [JsonProperty("d")]
        public int Direction { get; set; }
        [JsonProperty("l")]
        public int LoopCount { get; set; }
    }

    public class LightTranslationEvent
    {
        [JsonProperty("p")]
        public int TransitionType { get; set; }
        [JsonProperty("e")]
        public int Easing { get; set; }
        [JsonProperty("t")]
        public float Magnitude { get; set; }
    }
}
