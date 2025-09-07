using beatleader_parser.Timescale;
using beatleader_parser.VNJS;
using Newtonsoft.Json;
using Parser.Audio.V4;
using Parser.Map.Difficulty.V2.Base;
using Parser.Map.Difficulty.V3.Custom;
using Parser.Map.Difficulty.V3.Event;
using Parser.Map.Difficulty.V3.Event.V3;
using Parser.Map.Difficulty.V3.Grid;
using Parser.Map.Difficulty.V4.Base;
using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Base
{
    public class DifficultyV3
    {
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; } = "3.3.0";
        public List<BpmEvent> bpmEvents { get; set; } = new();
        public List<NjsEvent> njsEvents { get; set; } = new();
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
        public BasicEventTypesWithKeywords basicEventTypesWithKeywords { get; set; } = new (new List<BasicEventTypesWithKeywords.BasicEventTypesForKeyword> { });
        public bool useNormalEventsAsCompatibleEvents { get; set; }
        [JsonIgnore]
        public Custom.Customdata customData { get; set; }
        public object[] vfxEventBoxGroups { get; set; }
        [JsonIgnore]
        public _Fxeventscollection _fxEventsCollection { get; set; }

        public static DifficultyV3 V4toV3(DifficultyV4 v4, AudioData? audioData, Lighting? lighting)
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
                Rotations = new(),
                bpmEvents = new(),
            };

            // Convert notes
            var colorNotesData = v4.colorNotesData ?? new();
            if (v4.colorNotes != null)
            {
                foreach (var note in v4.colorNotes)
                {
                    var noteData = note.Index < colorNotesData.Count ? colorNotesData[note.Index] : new();
                    Note colorNote = new()
                    {
                        Beats = note.Beat,
                        x = noteData.X,
                        y = noteData.Y,
                        Color = noteData.Color,
                        CutDirection = noteData.Direction,
                        AngleOffset = noteData.AngleOffset,
                        customData = noteData.customData
                    };
                    difficultyV3.Notes.Add(colorNote);
                }
            }

            // Convert bombs
            var bombNotesData = v4.bombNotesData ?? new();
            if (v4.bombNotes != null)
            {
                foreach (var bomb in v4.bombNotes)
                {
                    var bombData = bomb.Index < bombNotesData.Count ? bombNotesData[bomb.Index] : new();
                    Bomb bombNote = new()
                    {
                        Beats = bomb.Beat,
                        x = bombData.X,
                        y = bombData.Y,
                        customData = bombData.customData
                    };
                    difficultyV3.Bombs.Add(bombNote);
                }
            }

            // Convert walls
            var obstaclesData = v4.obstaclesData ?? new();
            if (v4.obstacles != null)
            {
                foreach (var wall in v4.obstacles)
                {
                    var wallData = wall.Index < obstaclesData.Count ? obstaclesData[wall.Index] : new();
                    Wall obstacleNote = new()
                    {
                        Beats = wall.Beat,
                        x = wallData.X,
                        y = wallData.Y,
                        DurationInBeats = wallData.Duration,
                        Width = wallData.Width,
                        Height = wallData.Height
                    };
                    difficultyV3.Walls.Add(obstacleNote);
                }
            }

            // Convert arcs
            var arcsData = v4.arcsData ?? new();
            if (v4.arcs != null)
            {
                foreach (var arc in v4.arcs)
                {
                    var arcData = arc.ArcIndex < arcsData.Count ? arcsData[arc.ArcIndex] : new();
                    var headNoteData = arc.HeadIndex < colorNotesData.Count ? colorNotesData[arc.HeadIndex] : new();
                    var tailNoteData = arc.TailIndex < colorNotesData.Count ? colorNotesData[arc.TailIndex] : new();

                    Arc slider = new()
                    {
                        Beats = arc.HeadBeat,
                        x = headNoteData.X,
                        y = headNoteData.Y,
                        Color = headNoteData.Color,
                        CutDirection = headNoteData.Direction,
                        TailInBeats = arc.TailBeat,
                        tx = tailNoteData.X,
                        ty = tailNoteData.Y,
                        Multiplier = arcData.HeadControlPointLengthMultiplier,
                        TailMultiplier = arcData.TailControlPointLengthMultiplier,
                        TailDirection = tailNoteData.Direction,
                        AnchorMode = arcData.MidAnchorMode,
                        customData = headNoteData.customData != null || tailNoteData.customData != null ? new GridObjectCustomData {
                            coordinates = headNoteData.customData?.coordinates,
                            tailCoordinates = tailNoteData.customData?.coordinates,
                        } : null
                    };
                    difficultyV3.Arcs.Add(slider);
                }
            }

            // Convert chains
            var chainsData = v4.chainsData ?? new();
            if (v4.chains != null)
            {
                foreach (var chain in v4.chains)
                {
                    var chainData = chain.ChainIndex < chainsData.Count ? chainsData[chain.ChainIndex] : new();
                    var headNoteData = chain.Index < colorNotesData.Count ? colorNotesData[chain.Index] : new();

                    Chain burstSlider = new()
                    {
                        Beats = chain.HeadBeat,
                        x = headNoteData.X,
                        y = headNoteData.Y,
                        Color = headNoteData.Color,
                        CutDirection = headNoteData.Direction,
                        TailInBeats = chain.TailBeat,
                        tx = chainData.TailX,
                        ty = chainData.TailY,
                        SliceCount = chainData.SliceCount,
                        Squish = chainData.SquishFactor
                    };
                    difficultyV3.Chains.Add(burstSlider);
                }
            }

            // Convert rotations
            //var spawnRotationsData = v4.spawnRotationsData ?? new();
            //if (v4.spawnRotations != null)
            //{
            //    foreach (var rotation in v4.spawnRotations)
            //    {
            //        var rotationData = rotation.Index < spawnRotationsData.Count ? spawnRotationsData[rotation.Index] : new();
            //        var value = (Math.Abs(rotationData.Rotation) - 60) / -15;
                    
            //        RotationEvent evt = new()
            //        {
            //            Beats = rotation.Beat,
            //            ExecutionTime = rotationData.Type == 1 ? 15 : 14,
            //            Value = value < 4 ? value : value - 1,
            //            Inverted = rotationData.Rotation > 0
            //        };
            //        difficultyV3.Rotations.Add(evt);
            //    }
            //}

            // Convert BPM events from audio data
            if (audioData?.bpmData != null)
            {
                foreach (var bpmData in audioData.bpmData)
                {
                    float bpmChangeStartTime = bpmData.startSampleIndex / audioData.songFrequency;
                    float numSamples = bpmData.endSampleIndex - bpmData.startSampleIndex;
                    float bpm = ((bpmData.endBeat - bpmData.startBeat) / (numSamples / audioData.songFrequency)) * 60.0f;

                    BpmEvent evt = new()
                    {
                        Beats = bpmData.startBeat,
                        Bpm = bpm,
                        BpmChangeStartTime = bpmChangeStartTime
                    };
                    difficultyV3.bpmEvents.Add(evt);
                }
            }

            // Convert NJS events
            var njsEventsData = v4.njsEventData ?? new();
            if (v4.njsEvents != null)
            {
                NjsEvent? previousEvent = null;
                foreach (var njsEvent in v4.njsEvents)
                {
                    var eventData = njsEvent.Index < njsEventsData.Count ? njsEventsData[njsEvent.Index] : new();
                    NjsEvent evt = new()
                    {
                        Beats = njsEvent.Beat,
                        Delta = eventData.UsePrevious == 1 && previousEvent != null ? previousEvent.Delta : eventData.Delta,
                        Easing = eventData.Easing
                    };
                    difficultyV3.njsEvents.Add(evt);
                    previousEvent = evt;
                }
            }

            // Convert lighting box groups
            if (lighting?.eventBoxGroups != null)
            {
                foreach (var boxGroup in lighting.eventBoxGroups)
                {
                    switch (boxGroup.Type)
                    {
                        case 1: // Light Color
                            var colorGroup = new Lightcoloreventboxgroup
                            {
                                Beats = boxGroup.Beat,
                                Group = boxGroup.Group,
                                EventBoxGroup = new()
                            };

                            foreach (var box in boxGroup.Events)
                            {
                                var filter = box.FilterIndex < lighting.indexFilters.Count ? lighting.indexFilters[box.FilterIndex] : new();
                                var eventBox = box.EventBoxIndex < lighting.lightColorEventBoxes.Count ? lighting.lightColorEventBoxes[box.EventBoxIndex] : new();

                                var e = new E
                                {
                                    Filter = new F
                                    {
                                        f = filter.Type,
                                        p = filter.Parameter0,
                                        t = filter.Parameter1,
                                        r = filter.Reverse,
                                        c = filter.Chunks,
                                        n = filter.RandomBehavior,
                                        s = filter.RandomSeed,
                                        l = filter.LimitPercent,
                                        d = filter.LimitBehavior
                                    },
                                    w = eventBox.BeatDistributionValue,
                                    d = eventBox.BeatDistributionType,
                                    r = eventBox.BrightnessDistributionValue,
                                    t = eventBox.BrightnessDistributionType,
                                    b = eventBox.BrightnessDistributionAffectsFirst,
                                    i = eventBox.BrightnessDistributionEasing,
                                    e = new()
                                };

                                foreach (var evt in box.Events)
                                {
                                    var colorEvent = evt.Index < lighting.lightColorEvents.Count ? lighting.lightColorEvents[evt.Index] : new();
                                    e.e.Add(new E1
                                    {
                                        b = evt.Beat,
                                        c = colorEvent.Color,
                                        s = colorEvent.Brightness,
                                        i = colorEvent.TransitionType,
                                        f = colorEvent.StrobeFrequency,
                                        sb = colorEvent.StrobeBrightness,
                                        sf = colorEvent.StrobeFade
                                    });
                                }

                                colorGroup.EventBoxGroup.Add(e);
                            }

                            difficultyV3.lightColorEventBoxGroups.Add(colorGroup);
                            break;

                        case 2: // Light Rotation
                            var rotationGroup = new Lightrotationeventboxgroup
                            {
                                Beats = boxGroup.Beat,
                                Group = boxGroup.Group,
                                EventBoxGroup = new()
                            };

                            foreach (var box in boxGroup.Events)
                            {
                                var filter = box.FilterIndex < lighting.indexFilters.Count ? lighting.indexFilters[box.FilterIndex] : new();
                                var eventBox = box.EventBoxIndex < lighting.lightRotationEventBoxes.Count ? lighting.lightRotationEventBoxes[box.EventBoxIndex] : new();

                                var e = new E2
                                {
                                    f = new F
                                    {
                                        f = filter.Type,
                                        p = filter.Parameter0,
                                        t = filter.Parameter1,
                                        r = filter.Reverse,
                                        c = filter.Chunks,
                                        n = filter.RandomBehavior,
                                        s = filter.RandomSeed,
                                        l = filter.LimitPercent,
                                        d = filter.LimitBehavior
                                    },
                                    w = eventBox.BeatDistributionValue,
                                    d = eventBox.BeatDistributionType,
                                    s = eventBox.RotationDistributionValue,
                                    t = eventBox.RotationDistributionType,
                                    b = eventBox.RotationDistributionAffectsFirst,
                                    i = eventBox.RotationDistributionEasing,
                                    a = eventBox.Axis,
                                    r = eventBox.InvertAxis,
                                    l = new()
                                };

                                foreach (var evt in box.Events)
                                {
                                    var rotationEvent = evt.Index < lighting.lightRotationEvents.Count ? lighting.lightRotationEvents[evt.Index] : new();
                                    e.l.Add(new L
                                    {
                                        b = evt.Beat,
                                        r = rotationEvent.Magnitude,
                                        o = rotationEvent.Direction,
                                        e = rotationEvent.Easing,
                                        l = rotationEvent.LoopCount,
                                        p = rotationEvent.TransitionType
                                    });
                                }

                                rotationGroup.EventBoxGroup.Add(e);
                            }

                            difficultyV3.lightRotationEventBoxGroups.Add(rotationGroup);
                            break;

                        case 3: // Light Translation
                            var translationGroup = new Lighttranslationeventboxgroup
                            {
                                Beats = boxGroup.Beat,
                                Group = boxGroup.Group,
                                EventBoxGroup = new()
                            };

                            foreach (var box in boxGroup.Events)
                            {
                                var filter = box.FilterIndex < lighting.indexFilters.Count ? lighting.indexFilters[box.FilterIndex] : new();
                                var eventBox = box.EventBoxIndex < lighting.lightTranslationEventBoxes.Count ? lighting.lightTranslationEventBoxes[box.EventBoxIndex] : new();

                                var e = new E3
                                {
                                    f = new F
                                    {
                                        f = filter.Type,
                                        p = filter.Parameter0,
                                        t = filter.Parameter1,
                                        r = filter.Reverse,
                                        c = filter.Chunks,
                                        n = filter.RandomBehavior,
                                        s = filter.RandomSeed,
                                        l = filter.LimitPercent,
                                        d = filter.LimitBehavior
                                    },
                                    w = eventBox.BeatDistributionValue,
                                    d = eventBox.BeatDistributionType,
                                    s = eventBox.GapDistributionValue,
                                    t = eventBox.GapDistributionType,
                                    b = eventBox.GapDistributionAffectsFirst,
                                    i = eventBox.GapDistributionEasing,
                                    a = eventBox.Axis,
                                    r = eventBox.InvertAxis,
                                    l = new()
                                };

                                foreach (var evt in box.Events)
                                {
                                    var translationEvent = evt.Index < lighting.lightTranslationEvents.Count ? lighting.lightTranslationEvents[evt.Index] : new();
                                    e.l.Add(new L1
                                    {
                                        b = evt.Beat,
                                        p = translationEvent.TransitionType,
                                        e = translationEvent.Easing,
                                        t = translationEvent.Magnitude
                                    });
                                }

                                translationGroup.EventBoxGroup.Add(e);
                            }

                            difficultyV3.lightTranslationEventBoxGroups.Add(translationGroup);
                            break;
                    }
                }
            }

            return difficultyV3;
        }

        public static DifficultyV3 V2toV3(DifficultyV2 v2, float bpm, float njs)
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
                Rotations = new(),
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
                        AngleOffset = 0,
                        customData = note._customData != null ? new GridObjectCustomData {
                            coordinates = note._customData._position
                        } : null,
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
                        customData = note._customData != null ? new GridObjectCustomData {
                            coordinates = note._customData._position
                        } : null,
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

                if ((obstacle._type >= 1000 && obstacle._type <= 4000) || (obstacle._type >= 4001 && obstacle._type <= 4005000)) {
					int obsHeight = 0;
					int startHeight = 0;
					var value = obstacle._type;
					if (obstacle._type >= 4001 && obstacle._type <= 4100000) {
						value -= 4001;
						obsHeight = value / 1000;
						startHeight = value % 1000;
					} else {
						obsHeight = value - 1000;
					}

					var height = (obsHeight / 1000) * 5;
					height = height * 1000 + 1000;

					var layer = (startHeight / 750) * 5;
					layer = layer * 1000 + 1334;

					obs.y = layer / 1000 - 2;
					obs.Height = (height - 1000) / 1000;
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
                    TailMultiplier = arc._tailControlPointLengthMultiplier,
                    customData = arc._customData != null ? new GridObjectCustomData {
                        coordinates = arc._customData._position,
                        tailCoordinates = arc._customData._tailPosition
                    } : null,
                };
                difficultyV3.Arcs.Add(slider);
            }
            foreach (var ev in v2._events)
            {
                if(ev._type == 14 || ev._type == 15)
                {
                    RotationEvent rotation = new()
                    {
                        Beats = ev._time,
                        Rotation = ev._value,
                        Event = 1 // No idea if it's supposed to be 0 or 1
                    };
                    difficultyV3.Rotations.Add(rotation);
                }
                if(ev._type == 100)
                {
                    BpmEvent bpmEvent = new()
                    {
                        Beats = ev._time,
                        Bpm = ev._floatValue
                    };
                    difficultyV3.bpmEvents.Add(bpmEvent);
                }
                else
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
            }

            ConvertTime(difficultyV3, bpm);
            CalculateObjectNjs(difficultyV3, njs);

            return difficultyV3;
        }

        public static void ConvertTime(DifficultyV3 diff, float bpm)
        {
            List<BeatmapObject> obj = new();
            obj.AddRange(diff.Notes);
            obj.AddRange(diff.Bombs);
            obj.AddRange(diff.Lights);
            obj.AddRange(diff.bpmEvents);
            obj.AddRange(diff.njsEvents);
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

        public static void CalculateObjectNjs(DifficultyV3 diff, float baseNjs)
        {
            List<BeatmapGridObject> obj = new();
            obj.AddRange(diff.Notes);
            obj.AddRange(diff.Bombs);
            obj.AddRange(diff.Walls);
            obj.AddRange(diff.Arcs);
            obj.AddRange(diff.Chains);
            var vnjs = new VNJS(baseNjs, diff.njsEvents);
            vnjs.CalculateAllObjectNjs(obj);
        }
    }
}
