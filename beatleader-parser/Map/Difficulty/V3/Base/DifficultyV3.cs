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
        public string version { get; set; } = "3.3.0";
        public List<Bpmevent> bpmEvents { get; set; } = new();
        public List<RotationEvent> rotationEvents { get; set; } = new();
        public List<Colornote> colorNotes { get; set; } = new();
        public List<Bombnote> bombNotes { get; set; } = new();
        public List<Obstacle> obstacles { get; set; } = new();
        public List<Slider> sliders { get; set; } = new();
        public List<Burstslider> burstSliders { get; set; } = new();
        public object[] waypoints { get; set; }
        public List<Basicbeatmapevent> basicBeatmapEvents { get; set; } = new();
        public List<Colorboostbeatmapevent> colorBoostBeatmapEvents { get; set; } = new();
        public List<Lightcoloreventboxgroup> lightColorEventBoxGroups { get; set; } = new();
        public List<Lightrotationeventboxgroup> lightRotationEventBoxGroups { get; set; } = new();
        public List<Lighttranslationeventboxgroup> lightTranslationEventBoxGroups { get; set; } = new();
        public Basiceventtypeswithkeywords basicEventTypesWithKeywords { get; set; } = new();
        public bool useNormalEventsAsCompatibleEvents { get; set; }
        public Custom.Customdata customData { get; set; }
        public object[] vfxEventBoxGroups { get; set; }
        public _Fxeventscollection _fxEventsCollection { get; set; }

        public static DifficultyV3 V2toV3(DifficultyV2 v2)
        {
            DifficultyV3 difficultyV3 = new()
            {
                version = "3.0.0",
                colorNotes = new(),
                bombNotes = new(),
                burstSliders = new(),
                sliders = new(),
                obstacles = new(),
                basicBeatmapEvents = new(),
                lightColorEventBoxGroups = new(),
                bpmEvents = new(),
            };
            foreach (var note in v2._notes)
            {
                if (note._type == 0 || note._type == 1)
                {
                    Colornote colornote = new()
                    {
                        d = note._cutDirection,
                        c = note._type,
                        b = note._time,
                        x = note._lineIndex,
                        y = note._lineLayer,
                        a = 0
                    };
                    difficultyV3.colorNotes.Add(colornote);
                }
                else if (note._type == 3)
                {
                    Bombnote bombnote = new()
                    {
                        b = note._time,
                        x = note._lineIndex,
                        y = note._lineLayer,
                    };
                    difficultyV3.bombNotes.Add(bombnote);
                }
            }
            foreach (var obstacle in v2._obstacles)
            {
                Obstacle obs = new()
                {
                    b = obstacle._time,
                    x = obstacle._lineIndex,
                    w = obstacle._width,
                    d = obstacle._duration
                };
                if (obstacle._type == 0)
                {
                    obs.y = 0;
                    obs.h = 5;
                }
                else if (obstacle._type == 1)
                {
                    obs.y = 2;
                    obs.h = 3;
                }
                difficultyV3.obstacles.Add(obs);
            }
            foreach (var arc in v2._sliders)
            {
                Slider slider = new()
                {
                    c = arc.colorType,
                    d = arc._headCutDirection,
                    b = arc._headTime,
                    tb = arc._tailTime,
                    x = arc._headLineIndex,
                    tx = arc._tailLineIndex,
                    y = arc._headLineLayer,
                    ty = arc._tailLineLayer,
                    m = arc._sliderMidAnchorMode,
                    mu = arc._headControlPointLengthMultiplier,
                    tmu = arc._tailControlPointLengthMultiplier
                };
                difficultyV3.sliders.Add(slider);
            }
            foreach (var ev in v2._events)
            {
                Basicbeatmapevent basic = new()
                {
                    b = ev._time,
                    et = ev._type,
                    i = ev._value,
                    f = ev._floatValue
                };
                difficultyV3.basicBeatmapEvents.Add(basic);
            }
            return difficultyV3;
        }
    }
}
