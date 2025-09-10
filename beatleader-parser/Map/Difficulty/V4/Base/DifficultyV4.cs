using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Parser.Map.Difficulty.V4.Base
{
    public class DifficultyV4
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = "4.1.0";

        [JsonPropertyName("colorNotes")]
        public List<BaseNote> colorNotes { get; set; } = new();
        [JsonPropertyName("colorNotesData")] 
        public List<ColorNoteData> colorNotesData { get; set; } = new();
        [JsonPropertyName("bombNotes")]
        public List<BaseNote> bombNotes { get; set; } = new();
        [JsonPropertyName("bombNotesData")]
        public List<BombNoteData> bombNotesData { get; set; } = new();
        [JsonPropertyName("obstacles")]
        public List<BaseNote> obstacles { get; set; } = new();
        [JsonPropertyName("obstaclesData")]
        public List<ObstacleData> obstaclesData { get; set; } = new();
        [JsonPropertyName("arcs")]
        public List<ArcNote> arcs { get; set; } = new();
        [JsonPropertyName("arcsData")]
        public List<ArcData> arcsData { get; set; } = new();
        [JsonPropertyName("chains")]
        public List<ChainNote> chains { get; set; } = new();
        [JsonPropertyName("chainsData")]
        public List<ChainData> chainsData { get; set; } = new();
        [JsonPropertyName("spawnRotations")]
        public List<BaseEvent> spawnRotations { get; set; } = new();
        [JsonPropertyName("spawnRotationsData")]
        public List<RotationData> spawnRotationsData { get; set; } = new();
        [JsonPropertyName("njsEvents")]
        public List<BaseEvent> njsEvents { get; set; } = new();
        [JsonPropertyName("njsEventData")]
        public List<NjsEventData> njsEventData { get; set; } = new();
        [JsonPropertyName("waypoints")]
        public object[] Waypoints { get; set; }
    }

    public class Lighting {
        // V4 Lighting Box Groups
        [JsonPropertyName("eventBoxGroups")]
        public List<EventBoxGroup> eventBoxGroups { get; set; } = new();
        [JsonPropertyName("indexFilters")]
        public List<IndexFilter> indexFilters { get; set; } = new();
        [JsonPropertyName("lightColorEventBoxes")]
        public List<LightColorEventBox> lightColorEventBoxes { get; set; } = new();
        [JsonPropertyName("lightRotationEventBoxes")]
        public List<LightRotationEventBox> lightRotationEventBoxes { get; set; } = new();
        [JsonPropertyName("lightTranslationEventBoxes")]
        public List<LightTranslationEventBox> lightTranslationEventBoxes { get; set; } = new();
        [JsonPropertyName("lightColorEvents")]
        public List<LightColorEvent> lightColorEvents { get; set; } = new();
        [JsonPropertyName("lightRotationEvents")]
        public List<LightRotationEvent> lightRotationEvents { get; set; } = new();
        [JsonPropertyName("lightTranslationEvents")]
        public List<LightTranslationEvent> lightTranslationEvents { get; set; } = new();
    }

    public class BaseNote
    {
        [JsonPropertyName("b")]
        public float Beat { get; set; }
        [JsonPropertyName("r")]
        public int RotationLane { get; set; }
        [JsonPropertyName("i")]
        public int Index { get; set; }
    }

    public class ColorNoteData 
    {
        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
        [JsonPropertyName("c")]
        public int Color { get; set; }
        [JsonPropertyName("d")]
        public int Direction { get; set; }
        [JsonPropertyName("a")]
        public int AngleOffset { get; set; }
        public GridObjectCustomData? customData { get; set; }
    }

    public class BombNoteData
    {
        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
        public GridObjectCustomData? customData { get; set; }
    }

    public class ObstacleData
    {
        [JsonPropertyName("d")]
        public float Duration { get; set; }
        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
        [JsonPropertyName("w")]
        public int Width { get; set; }
        [JsonPropertyName("h")]
        public int Height { get; set; }
    }

    public class ArcNote
    {
        [JsonPropertyName("hb")]
        public float HeadBeat { get; set; }
        [JsonPropertyName("tb")]
        public float TailBeat { get; set; }
        [JsonPropertyName("hr")]
        public int HeadRow { get; set; }
        [JsonPropertyName("tr")]
        public int TailRow { get; set; }
        [JsonPropertyName("hi")]
        public int HeadIndex { get; set; }
        [JsonPropertyName("ti")]
        public int TailIndex { get; set; }
        [JsonPropertyName("ai")]
        public int ArcIndex { get; set; }
    }

    public class ArcData
    {
        [JsonPropertyName("m")]
        public float HeadControlPointLengthMultiplier { get; set; }
        [JsonPropertyName("tm")]
        public float TailControlPointLengthMultiplier { get; set; }
        [JsonPropertyName("a")]
        public int MidAnchorMode { get; set; }
        public GridObjectCustomData? customData { get; set; }
    }

    public class ChainNote
    {
        [JsonPropertyName("hb")]
        public float HeadBeat { get; set; }
        [JsonPropertyName("tb")]
        public float TailBeat { get; set; }
        [JsonPropertyName("hr")]
        public int HeadRow { get; set; }
        [JsonPropertyName("tr")]
        public int TailRow { get; set; }
        [JsonPropertyName("i")]
        public int Index { get; set; }
        [JsonPropertyName("ci")]
        public int ChainIndex { get; set; }
    }

    public class ChainData
    {
        [JsonPropertyName("tx")]
        public int TailX { get; set; }
        [JsonPropertyName("ty")]
        public int TailY { get; set; }
        [JsonPropertyName("c")]
        public int SliceCount { get; set; }
        [JsonPropertyName("s")]
        public float SquishFactor { get; set; }
        public GridObjectCustomData? customData { get; set; }
    }

    public class BaseEvent
    {
        [JsonPropertyName("b")]
        public float Beat { get; set; }
        [JsonPropertyName("i")]
        public int Index { get; set; }
    }

    public class RotationData
    {
        [JsonPropertyName("t")]
        public int Type { get; set; }
        [JsonPropertyName("r")]
        public float Rotation { get; set; }
    }

    public class NjsEventData {
        [JsonPropertyName("d")]
        public float Delta { get; set; }
        [JsonPropertyName("p")]
        public int UsePrevious { get; set; }
        [JsonPropertyName("e")]
        public int Easing { get; set; }
    }

    // V4 Event Box Group Classes
    public class EventBoxGroup
    {
        [JsonPropertyName("b")]
        public float Beat { get; set; }
        [JsonPropertyName("g")]
        public int Group { get; set; }
        [JsonPropertyName("t")]
        public int Type { get; set; }
        [JsonPropertyName("e")]
        public List<EventBox> Events { get; set; } = new();
    }

    public class EventBox
    {
        [JsonPropertyName("f")]
        public int FilterIndex { get; set; }
        [JsonPropertyName("e")]
        public int EventBoxIndex { get; set; }
        [JsonPropertyName("l")]
        public List<BaseEvent> Events { get; set; } = new();
    }

    public class IndexFilter
    {
        [JsonPropertyName("c")]
        public int Chunks { get; set; }
        [JsonPropertyName("f")]
        public int Type { get; set; }
        [JsonPropertyName("p")]
        public int Parameter0 { get; set; }
        [JsonPropertyName("t")]
        public int Parameter1 { get; set; }
        [JsonPropertyName("r")]
        public int Reverse { get; set; }
        [JsonPropertyName("n")]
        public int RandomBehavior { get; set; }
        [JsonPropertyName("s")]
        public int RandomSeed { get; set; }
        [JsonPropertyName("l")]
        public float LimitPercent { get; set; }
        [JsonPropertyName("d")]
        public int LimitBehavior { get; set; }
    }

    public class LightColorEventBox
    {
        [JsonPropertyName("w")]
        public float BeatDistributionValue { get; set; }
        [JsonPropertyName("d")]
        public int BeatDistributionType { get; set; }
        [JsonPropertyName("s")]
        public float BrightnessDistributionValue { get; set; }
        [JsonPropertyName("t")]
        public int BrightnessDistributionType { get; set; }
        [JsonPropertyName("b")]
        public int BrightnessDistributionAffectsFirst { get; set; }
        [JsonPropertyName("e")]
        public int BrightnessDistributionEasing { get; set; }
    }

    public class LightRotationEventBox
    {
        [JsonPropertyName("w")]
        public float BeatDistributionValue { get; set; }
        [JsonPropertyName("d")]
        public int BeatDistributionType { get; set; }
        [JsonPropertyName("s")]
        public float RotationDistributionValue { get; set; }
        [JsonPropertyName("t")]
        public int RotationDistributionType { get; set; }
        [JsonPropertyName("b")]
        public int RotationDistributionAffectsFirst { get; set; }
        [JsonPropertyName("e")]
        public int RotationDistributionEasing { get; set; }
        [JsonPropertyName("a")]
        public int Axis { get; set; }
        [JsonPropertyName("f")]
        public int InvertAxis { get; set; }
    }

    public class LightTranslationEventBox
    {
        [JsonPropertyName("w")]
        public float BeatDistributionValue { get; set; }
        [JsonPropertyName("d")]
        public int BeatDistributionType { get; set; }
        [JsonPropertyName("s")]
        public float GapDistributionValue { get; set; }
        [JsonPropertyName("t")]
        public int GapDistributionType { get; set; }
        [JsonPropertyName("b")]
        public int GapDistributionAffectsFirst { get; set; }
        [JsonPropertyName("e")]
        public int GapDistributionEasing { get; set; }
        [JsonPropertyName("a")]
        public int Axis { get; set; }
        [JsonPropertyName("f")]
        public int InvertAxis { get; set; }
    }

    public class LightColorEvent
    {
        [JsonPropertyName("p")]
        public int TransitionType { get; set; }
        [JsonPropertyName("e")]
        public int Easing { get; set; }
        [JsonPropertyName("c")]
        public int Color { get; set; }
        [JsonPropertyName("b")]
        public float Brightness { get; set; }
        [JsonPropertyName("f")]
        public int StrobeFrequency { get; set; }
        [JsonPropertyName("sb")]
        public float StrobeBrightness { get; set; }
        [JsonPropertyName("sf")]
        public int StrobeFade { get; set; }
    }

    public class LightRotationEvent
    {
        [JsonPropertyName("p")]
        public int TransitionType { get; set; }
        [JsonPropertyName("e")]
        public int Easing { get; set; }
        [JsonPropertyName("r")]
        public float Magnitude { get; set; }
        [JsonPropertyName("d")]
        public int Direction { get; set; }
        [JsonPropertyName("l")]
        public int LoopCount { get; set; }
    }

    public class LightTranslationEvent
    {
        [JsonPropertyName("p")]
        public int TransitionType { get; set; }
        [JsonPropertyName("e")]
        public int Easing { get; set; }
        [JsonPropertyName("t")]
        public float Magnitude { get; set; }
    }
}
