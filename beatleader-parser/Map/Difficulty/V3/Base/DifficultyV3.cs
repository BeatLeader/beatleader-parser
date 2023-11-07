using beatleader_parser.Timescale;
using Newtonsoft.Json;
using Parser.Map.Difficulty.V2.Base;
using Parser.Map.Difficulty.V3.Custom;
using Parser.Map.Difficulty.V3.Event;
using Parser.Map.Difficulty.V3.Event.V3;
using Parser.Map.Difficulty.V3.Grid;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Base
{
    public class DifficultyV3
    {
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; } = "3.3.0";
        public List<BpmEvent> bpmEvents { get; set; } = new();
        [JsonProperty(PropertyName = "rotationEvents")]
        public List<RotationEvent> Rotations { get; set; } = new();
        [JsonProperty(PropertyName = "colorNotes")]
        public List<Note> Notes { get; set; } = new();
        [JsonProperty(PropertyName = "bombNotes")]
        public List<Bomb> Bombs { get; set; } = new();
        [JsonProperty(PropertyName = "obstacles")]
        public List<Wall> Walls { get; set; } = new();
        [JsonProperty(PropertyName = "sliders")]
        public List<Arc> Arcs { get; set; } = new();
        [JsonProperty(PropertyName = "burstSliders")]
        public List<Chain> Chains { get; set; } = new();
        [JsonProperty(PropertyName = "waypoints")]
        public object[] Waypoints { get; set; }
        [JsonProperty(PropertyName = "basicBeatmapEvents")]
        public List<Light> Lights { get; set; } = new();
        public List<Colorboostbeatmapevent> colorBoostBeatmapEvents { get; set; } = new();
        public List<Lightcoloreventboxgroup> lightColorEventBoxGroups { get; set; } = new();
        public List<Lightrotationeventboxgroup> lightRotationEventBoxGroups { get; set; } = new();
        public List<Lighttranslationeventboxgroup> lightTranslationEventBoxGroups { get; set; } = new();
        public Basiceventtypeswithkeywords basicEventTypesWithKeywords { get; set; } = new();
        public bool useNormalEventsAsCompatibleEvents { get; set; }
        public Custom.Customdata customData { get; set; }
        public object[] vfxEventBoxGroups { get; set; }
        public _Fxeventscollection _fxEventsCollection { get; set; }

        public static DifficultyV3 V2toV3(DifficultyV2 v2, float bpm)
        {
            DifficultyV3 difficultyV3 = new()
            {
                Version = "3.0.0",
                Notes = new(),
                Bombs = new(),
                Chains = new(),
                Arcs = new(),
                Walls = new(),
                Lights = new(),
                lightColorEventBoxGroups = new(),
                bpmEvents = new(),
            };
            foreach (var note in v2._notes)
            {
                if (note._type == 0 || note._type == 1)
                {
                    Note colornote = new()
                    {
                        CutDirection = note._cutDirection,
                        Color = note._type,
                        Beats = note._time,
                        x = note._lineIndex,
                        y = note._lineLayer,
                        AngleOffset = 0
                    };
                    difficultyV3.Notes.Add(colornote);
                }
                else if (note._type == 3)
                {
                    Bomb bombnote = new()
                    {
                        Beats = note._time,
                        x = note._lineIndex,
                        y = note._lineLayer,
                    };
                    difficultyV3.Bombs.Add(bombnote);
                }
            }
            foreach (var obstacle in v2._obstacles)
            {
                Wall obs = new()
                {
                    Beats = obstacle._time,
                    x = obstacle._lineIndex,
                    Width = obstacle._width,
                    DurationInBeats = obstacle._duration
                };
                if (obstacle._type == 0)
                {
                    obs.y = 0;
                    obs.Height = 5;
                }
                else if (obstacle._type == 1)
                {
                    obs.y = 2;
                    obs.Height = 3;
                }
                difficultyV3.Walls.Add(obs);
            }
            foreach (var arc in v2._sliders)
            {
                Arc slider = new()
                {
                    Color = arc.colorType,
                    CutDirection = arc._headCutDirection,
                    Beats = arc._headTime,
                    TailInBeats = arc._tailTime,
                    x = arc._headLineIndex,
                    tx = arc._tailLineIndex,
                    y = arc._headLineLayer,
                    ty = arc._tailLineLayer,
                    AnchorMode = arc._sliderMidAnchorMode,
                    Multiplier = arc._headControlPointLengthMultiplier,
                    TailMultiplier = arc._tailControlPointLengthMultiplier
                };
                difficultyV3.Arcs.Add(slider);
            }
            foreach (var ev in v2._events)
            {
                Light basic = new()
                {
                    Beats = ev._time,
                    Type = ev._type,
                    Value = ev._value,
                    f = ev._floatValue
                };
                difficultyV3.Lights.Add(basic);
            }

            ConvertTime(difficultyV3, bpm);

            return difficultyV3;
        }

        public static void ConvertTime(DifficultyV3 diff, float bpm)
        {
            List<BeatmapObject> obj = new();
            obj.AddRange(diff.Notes);
            obj.AddRange(diff.Bombs);
            obj.AddRange(diff.Lights);
            obj.AddRange(diff.bpmEvents);
            obj.AddRange(diff.Walls);
            obj.AddRange(diff.Arcs);
            obj.AddRange(diff.Chains);
            obj.AddRange(diff.Rotations);
            obj.AddRange(diff.colorBoostBeatmapEvents);
            obj.AddRange(diff.lightColorEventBoxGroups);
            obj.AddRange(diff.lightRotationEventBoxGroups);
            obj.AddRange(diff.lightTranslationEventBoxGroups);
            var timescale = Timescale.Create(bpm, diff.bpmEvents, 0);
            timescale.ConvertAllBeatsToSeconds(obj);
            timescale.ConvertAllBeatsToSeconds(diff.Chains);
            timescale.ConvertAllBeatsToSeconds(diff.Arcs);
            timescale.ConvertAllBeatsToSeconds(diff.Walls);
        }
    }
}
