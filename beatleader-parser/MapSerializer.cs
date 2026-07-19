using beatleader_parser.Utils;
using Parser.Audio.V4;
using Parser.Json;
using Parser.Map;
using Parser.Map.Difficulty.V3.Base;
using Parser.Map.Difficulty.V3.Event;
using Parser.Map.Difficulty.V3.Event.V3;
using Parser.Map.Difficulty.V3.Grid;
using Parser.Map.Difficulty.V4.Base;
using Parser.Map.V4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace beatleader_parser
{
    public class V4DifficultyExport
    {
        public string Characteristic { get; set; } = "";
        public string Difficulty { get; set; } = "";
        public DifficultyBeatmap Metadata { get; set; } = new();
        public DifficultyV4 Beatmap { get; set; } = new();
        public Lighting Lightshow { get; set; } = new();
    }

    public class V4MapExport
    {
        public InfoV4 Info { get; set; } = new();
        public AudioData AudioData { get; set; } = new();
        public List<V4DifficultyExport> Difficulties { get; set; } = new();
    }

    public static class MapSerializer
    {
        public static V4MapExport ExportV4(
            BeatmapV3 beatmap,
            string audioDataFilename = "AudioData.dat",
            int songFrequency = 44100,
            string songChecksum = "")
        {
            if (beatmap == null)
            {
                throw new ArgumentNullException(nameof(beatmap));
            }

            var orderedDifficulties = beatmap.Difficulties
                .OrderBy(d => d.Characteristic)
                .ThenBy(d => GetDifficultyRank(d.Difficulty))
                .ThenBy(d => d.Difficulty)
                .ToList();

            var exportedDifficulties = orderedDifficulties
                .Select(d => CreateDifficultyExport(d, beatmap.Info?._levelAuthorName))
                .ToList();

            return new V4MapExport
            {
                Info = CreateInfoV4(beatmap, exportedDifficulties, audioDataFilename),
                AudioData = CreateAudioData(beatmap, songFrequency, songChecksum),
                Difficulties = exportedDifficulties
            };
        }

        public static IReadOnlyList<(string Filename, string Json)> SerializeV4(
            BeatmapV3 beatmap,
            bool writeIndented = false,
            string audioDataFilename = "AudioData.dat",
            int songFrequency = 44100,
            string songChecksum = "")
        {
            var export = ExportV4(beatmap, audioDataFilename, songFrequency, songChecksum);
            var files = new List<(string Filename, string Json)>
            {
                ("Info.dat", JsonSerializerHelper.Serialize(export.Info, SerializeV4Context.Default.InfoV4, writeIndented)),
                (audioDataFilename, JsonSerializerHelper.Serialize(export.AudioData, SerializeV4Context.Default.AudioData, writeIndented))
            };
            var writtenFiles = new HashSet<string>(files.Select(file => file.Filename), StringComparer.OrdinalIgnoreCase);

            foreach (var difficulty in export.Difficulties)
            {
                if (writtenFiles.Add(difficulty.Metadata.beatmapDataFilename))
                {
                    files.Add((difficulty.Metadata.beatmapDataFilename, JsonSerializerHelper.Serialize(difficulty.Beatmap, SerializeV4Context.Default.DifficultyV4, writeIndented)));
                }

                if (writtenFiles.Add(difficulty.Metadata.lightshowDataFilename))
                {
                    files.Add((difficulty.Metadata.lightshowDataFilename, JsonSerializerHelper.Serialize(difficulty.Lightshow, SerializeV4Context.Default.Lighting, writeIndented)));
                }
            }

            return files;
        }

        public static void SaveV4(
            BeatmapV3 beatmap,
            string folderPath,
            bool writeIndented = false,
            string audioDataFilename = "AudioData.dat",
            int songFrequency = 44100,
            string songChecksum = "")
        {
            Directory.CreateDirectory(folderPath);

            foreach (var (filename, json) in SerializeV4(beatmap, writeIndented, audioDataFilename, songFrequency, songChecksum))
            {
                File.WriteAllText(Path.Combine(folderPath, filename), json);
            }
        }

        private static V4DifficultyExport CreateDifficultyExport(DifficultySet difficultySet, string? levelAuthorName)
        {
            var beatmapFilename = GetBeatmapFilename(difficultySet);
            var lightshowFilename = GetLightshowFilename(difficultySet, beatmapFilename);

            return new V4DifficultyExport
            {
                Characteristic = difficultySet.Characteristic,
                Difficulty = difficultySet.Difficulty,
                Metadata = new DifficultyBeatmap
                {
                    characteristic = difficultySet.Characteristic,
                    difficulty = difficultySet.Difficulty,
                    beatmapAuthors = CreateBeatmapAuthors(levelAuthorName),
                    environmentNameIdx = difficultySet.BeatMap?._environmentNameIdx ?? 0,
                    beatmapColorSchemeIdx = difficultySet.BeatMap?._beatmapColorSchemeIdx ?? 0,
                    noteJumpMovementSpeed = difficultySet.BeatMap?._noteJumpMovementSpeed ?? 10,
                    noteJumpStartBeatOffset = difficultySet.BeatMap?._noteJumpStartBeatOffset ?? 0,
                    beatmapDataFilename = beatmapFilename,
                    lightshowDataFilename = lightshowFilename
                },
                Beatmap = CreateDifficultyV4(difficultySet.Data),
                Lightshow = CreateLightingV4(difficultySet.Data)
            };
        }

        private static InfoV4 CreateInfoV4(BeatmapV3 beatmap, IReadOnlyList<V4DifficultyExport> difficulties, string audioDataFilename)
        {
            var info = beatmap.Info ?? new Info();
            var songDuration = GetSongDuration(beatmap);

            return new InfoV4
            {
                version = GetInfoVersion(info),
                song = new Song
                {
                    title = info._songName ?? "",
                    subTitle = info._songSubName ?? "",
                    author = info._songAuthorName ?? ""
                },
                audio = new Audio
                {
                    songFilename = info._songFilename ?? "",
                    songDuration = songDuration,
                    audioDataFilename = audioDataFilename,
                    bpm = info._beatsPerMinute,
                    lufs = 0,
                    previewStartTime = info._previewStartTime,
                    previewDuration = info._previewDuration
                },
                songPreviewFilename = info._songFilename ?? "",
                coverImageFilename = info._coverImageFilename ?? "",
                environmentNames = GetEnvironmentNames(info),
                colorSchemes = GetColorSchemes(info),
                difficultyBeatmaps = difficulties.Select(d => d.Metadata).ToArray()
            };
        }

        private static AudioData CreateAudioData(BeatmapV3 beatmap, int songFrequency, string songChecksum)
        {
            var referenceDifficulty = beatmap.Difficulties.FirstOrDefault();
            var referenceData = referenceDifficulty?.Data ?? new DifficultyV3();
            var songDuration = GetSongDuration(beatmap);

            return new AudioData
            {
                version = "4.0.0",
                songChecksum = songChecksum ?? "",
                songSampleCount = Math.Max(0, (int)Math.Round(songDuration * songFrequency)),
                songFrequency = songFrequency,
                bpmData = BuildBpmData(beatmap.Info?._beatsPerMinute ?? 0, referenceData.bpmEvents, songDuration, songFrequency),
                lufsData = new List<object>()
            };
        }

        private static DifficultyV4 CreateDifficultyV4(DifficultyV3 diff)
        {
            var v4 = new DifficultyV4
            {
                Version = diff.njsEvents.Count > 0 ? "4.1.0" : "4.0.0",
                Waypoints = diff.Waypoints ?? Array.Empty<object>()
            };

            foreach (var note in diff.Notes.OrderBy(n => n.Beats))
            {
                var index = v4.colorNotesData.Count;
                v4.colorNotesData.Add(new ColorNoteData
                {
                    X = note.x,
                    Y = note.y,
                    Color = note.Color,
                    Direction = note.CutDirection,
                    AngleOffset = (int)Math.Round(note.AngleOffset),
                    customData = note.customData
                });
                v4.colorNotes.Add(new BaseNote
                {
                    Beat = note.Beats,
                    RotationLane = 0,
                    Index = index
                });
            }

            foreach (var bomb in diff.Bombs.OrderBy(b => b.Beats))
            {
                var index = v4.bombNotesData.Count;
                v4.bombNotesData.Add(new BombNoteData
                {
                    X = bomb.x,
                    Y = bomb.y,
                    customData = bomb.customData
                });
                v4.bombNotes.Add(new BaseNote
                {
                    Beat = bomb.Beats,
                    RotationLane = 0,
                    Index = index
                });
            }

            foreach (var wall in diff.Walls.OrderBy(w => w.Beats))
            {
                var index = v4.obstaclesData.Count;
                v4.obstaclesData.Add(new ObstacleData
                {
                    Duration = wall.DurationInBeats,
                    X = wall.x,
                    Y = wall.y,
                    Width = wall.Width,
                    Height = wall.Height
                });
                v4.obstacles.Add(new BaseNote
                {
                    Beat = wall.Beats,
                    RotationLane = 0,
                    Index = index
                });
            }

            foreach (var arc in diff.Arcs.OrderBy(a => a.Beats))
            {
                var headIndex = FindOrAddColorMetadata(v4, arc.Beats, arc.x, arc.y, arc.Color, arc.CutDirection, 0, arc.customData?.coordinates);
                var tailIndex = FindOrAddColorMetadata(v4, arc.TailInBeats, arc.tx, arc.ty, arc.Color, arc.TailCutDirection, 0, arc.customData?.tailCoordinates);
                var arcIndex = v4.arcsData.Count;

                v4.arcsData.Add(new ArcData
                {
                    HeadControlPointLengthMultiplier = arc.Multiplier,
                    TailControlPointLengthMultiplier = arc.TailMultiplier,
                    MidAnchorMode = arc.AnchorMode
                });

                v4.arcs.Add(new ArcNote
                {
                    HeadBeat = arc.Beats,
                    TailBeat = arc.TailInBeats,
                    HeadRow = 0,
                    TailRow = 0,
                    HeadIndex = headIndex,
                    TailIndex = tailIndex,
                    ArcIndex = arcIndex
                });
            }

            foreach (var chain in diff.Chains.OrderBy(c => c.Beats))
            {
                var headIndex = FindOrAddColorMetadata(v4, chain.Beats, chain.x, chain.y, chain.Color, chain.CutDirection, 0, chain.customData?.coordinates);
                var chainIndex = v4.chainsData.Count;

                v4.chainsData.Add(new ChainData
                {
                    TailX = chain.tx,
                    TailY = chain.ty,
                    SliceCount = chain.SliceCount,
                    SquishFactor = chain.Squish,
                    customData = chain.customData
                });

                v4.chains.Add(new ChainNote
                {
                    HeadBeat = chain.Beats,
                    TailBeat = chain.TailInBeats,
                    HeadRow = 0,
                    TailRow = 0,
                    Index = headIndex,
                    ChainIndex = chainIndex
                });
            }

            foreach (var rotation in diff.Rotations.OrderBy(r => r.Beats))
            {
                var index = v4.spawnRotationsData.Count;
                v4.spawnRotationsData.Add(new RotationData
                {
                    Type = rotation.Event,
                    Rotation = rotation.Rotation
                });
                v4.spawnRotations.Add(new BaseEvent
                {
                    Beat = rotation.Beats,
                    Index = index
                });
            }

            foreach (var njsEvent in diff.njsEvents.OrderBy(e => e.Beats))
            {
                var index = v4.njsEventData.Count;
                v4.njsEventData.Add(new NjsEventData
                {
                    Delta = njsEvent.Delta,
                    UsePrevious = njsEvent.UsePrevious,
                    Easing = njsEvent.Easing
                });
                v4.njsEvents.Add(new BaseEvent
                {
                    Beat = njsEvent.Beats,
                    Index = index
                });
            }

            return v4;
        }

        private static Lighting CreateLightingV4(DifficultyV3 diff)
        {
            var lighting = new Lighting
            {
                Version = "4.0.0",
                basicEventTypesWithKeywords = diff.basicEventTypesWithKeywords
            };

            foreach (var light in diff.Lights.OrderBy(l => l.Beats).ThenBy(l => l.Type))
            {
                var index = lighting.basicEventsData.Count;
                lighting.basicEventsData.Add(new BasicEventData
                {
                    Type = light.Type,
                    Value = light.Value,
                    FloatValue = light.f
                });
                lighting.basicEvents.Add(new BaseEvent
                {
                    Beat = light.Beats,
                    Index = index
                });
            }

            foreach (var colorBoost in diff.colorBoostBeatmapEvents.OrderBy(e => e.Beats))
            {
                var index = lighting.colorBoostEventsData.Count;
                lighting.colorBoostEventsData.Add(new ColorBoostEventData
                {
                    Boost = colorBoost.On ? 1 : 0
                });
                lighting.colorBoostEvents.Add(new BaseEvent
                {
                    Beat = colorBoost.Beats,
                    Index = index
                });
            }

            foreach (var group in diff.lightColorEventBoxGroups.OrderBy(g => g.Beats))
            {
                AddColorEventBoxGroup(lighting, group);
            }

            foreach (var group in diff.lightRotationEventBoxGroups.OrderBy(g => g.Beats))
            {
                AddRotationEventBoxGroup(lighting, group);
            }

            foreach (var group in diff.lightTranslationEventBoxGroups.OrderBy(g => g.Beats))
            {
                AddTranslationEventBoxGroup(lighting, group);
            }

            return lighting;
        }

        private static void AddColorEventBoxGroup(Lighting lighting, Lightcoloreventboxgroup group)
        {
            var eventGroup = new EventBoxGroup
            {
                Beat = group.Beats,
                Group = group.Group,
                Type = 1
            };

            foreach (var eventBox in group.EventBoxGroup ?? new List<E>())
            {
                var filterIndex = AddFilter(lighting, eventBox.Filter);
                var lightColorEventBoxIndex = lighting.lightColorEventBoxes.Count;
                lighting.lightColorEventBoxes.Add(new LightColorEventBox
                {
                    BeatDistributionValue = eventBox.w,
                    BeatDistributionType = eventBox.d,
                    BrightnessDistributionValue = eventBox.r,
                    BrightnessDistributionType = eventBox.t,
                    BrightnessDistributionAffectsFirst = eventBox.b,
                    BrightnessDistributionEasing = eventBox.i
                });

                var baseEvents = new List<BaseEvent>();
                foreach (var evt in eventBox.e ?? new List<E1>())
                {
                    var colorEventIndex = lighting.lightColorEvents.Count;
                    lighting.lightColorEvents.Add(new LightColorEvent
                    {
                        TransitionType = evt.i,
                        Easing = -1,
                        Color = evt.c,
                        Brightness = evt.s,
                        StrobeFrequency = evt.f,
                        StrobeBrightness = evt.sb,
                        StrobeFade = evt.sf
                    });

                    baseEvents.Add(new BaseEvent
                    {
                        Beat = evt.b,
                        Index = colorEventIndex
                    });
                }

                eventGroup.Events.Add(new EventBox
                {
                    FilterIndex = filterIndex,
                    EventBoxIndex = lightColorEventBoxIndex,
                    Events = baseEvents
                });
            }

            lighting.eventBoxGroups.Add(eventGroup);
        }

        private static void AddRotationEventBoxGroup(Lighting lighting, Lightrotationeventboxgroup group)
        {
            var eventGroup = new EventBoxGroup
            {
                Beat = group.Beats,
                Group = group.Group,
                Type = 2
            };

            foreach (var eventBox in group.EventBoxGroup ?? new List<E2>())
            {
                var filterIndex = AddFilter(lighting, eventBox.f);
                var lightRotationEventBoxIndex = lighting.lightRotationEventBoxes.Count;
                lighting.lightRotationEventBoxes.Add(new LightRotationEventBox
                {
                    BeatDistributionValue = eventBox.w,
                    BeatDistributionType = eventBox.d,
                    RotationDistributionValue = eventBox.s,
                    RotationDistributionType = eventBox.t,
                    RotationDistributionAffectsFirst = eventBox.b,
                    RotationDistributionEasing = eventBox.i,
                    Axis = eventBox.a,
                    InvertAxis = eventBox.r
                });

                var baseEvents = new List<BaseEvent>();
                foreach (var evt in eventBox.l ?? new List<L>())
                {
                    var rotationEventIndex = lighting.lightRotationEvents.Count;
                    lighting.lightRotationEvents.Add(new LightRotationEvent
                    {
                        TransitionType = evt.p,
                        Easing = evt.e,
                        Magnitude = evt.r,
                        Direction = evt.o,
                        LoopCount = evt.l
                    });

                    baseEvents.Add(new BaseEvent
                    {
                        Beat = evt.b,
                        Index = rotationEventIndex
                    });
                }

                eventGroup.Events.Add(new EventBox
                {
                    FilterIndex = filterIndex,
                    EventBoxIndex = lightRotationEventBoxIndex,
                    Events = baseEvents
                });
            }

            lighting.eventBoxGroups.Add(eventGroup);
        }

        private static void AddTranslationEventBoxGroup(Lighting lighting, Lighttranslationeventboxgroup group)
        {
            var eventGroup = new EventBoxGroup
            {
                Beat = group.Beats,
                Group = group.Group,
                Type = 3
            };

            foreach (var eventBox in group.EventBoxGroup ?? new List<E3>())
            {
                var filterIndex = AddFilter(lighting, eventBox.f);
                var lightTranslationEventBoxIndex = lighting.lightTranslationEventBoxes.Count;
                lighting.lightTranslationEventBoxes.Add(new LightTranslationEventBox
                {
                    BeatDistributionValue = eventBox.w,
                    BeatDistributionType = eventBox.d,
                    GapDistributionValue = eventBox.s,
                    GapDistributionType = eventBox.t,
                    GapDistributionAffectsFirst = eventBox.b,
                    GapDistributionEasing = eventBox.i,
                    Axis = eventBox.a,
                    InvertAxis = eventBox.r
                });

                var baseEvents = new List<BaseEvent>();
                foreach (var evt in eventBox.l ?? new List<L1>())
                {
                    var translationEventIndex = lighting.lightTranslationEvents.Count;
                    lighting.lightTranslationEvents.Add(new LightTranslationEvent
                    {
                        TransitionType = evt.p,
                        Easing = evt.e,
                        Magnitude = evt.t
                    });

                    baseEvents.Add(new BaseEvent
                    {
                        Beat = evt.b,
                        Index = translationEventIndex
                    });
                }

                eventGroup.Events.Add(new EventBox
                {
                    FilterIndex = filterIndex,
                    EventBoxIndex = lightTranslationEventBoxIndex,
                    Events = baseEvents
                });
            }

            lighting.eventBoxGroups.Add(eventGroup);
        }

        private static int AddFilter(Lighting lighting, F? filter)
        {
            lighting.indexFilters.Add(new IndexFilter
            {
                Chunks = filter?.c ?? 0,
                Type = filter?.f ?? 0,
                Parameter0 = filter?.p ?? 0,
                Parameter1 = filter?.t ?? 0,
                Reverse = filter?.r ?? 0,
                RandomBehavior = filter?.n ?? 0,
                RandomSeed = filter?.s ?? 0,
                LimitPercent = filter?.l ?? 0,
                LimitBehavior = filter?.d ?? 0
            });

            return lighting.indexFilters.Count - 1;
        }

        private static int FindOrAddColorMetadata(DifficultyV4 v4, float beat, int x, int y, int color, int direction, float angleOffset, float[]? coordinates)
        {
            var roundedAngleOffset = (int)Math.Round(angleOffset);

            foreach (var note in v4.colorNotes)
            {
                if (!ApproximatelyEqual(note.Beat, beat) || note.Index < 0 || note.Index >= v4.colorNotesData.Count)
                {
                    continue;
                }

                var noteData = v4.colorNotesData[note.Index];
                if (ColorMetadataMatches(noteData, x, y, coordinates))
                {
                    return note.Index;
                }
            }

            v4.colorNotesData.Add(new ColorNoteData
            {
                X = x,
                Y = y,
                Color = color,
                Direction = direction,
                AngleOffset = roundedAngleOffset,
                customData = coordinates == null ? null : new GridObjectCustomData { coordinates = coordinates }
            });

            return v4.colorNotesData.Count - 1;
        }

        private static bool ColorMetadataMatches(
            ColorNoteData data,
            int x,
            int y,
            float[]? coordinates)
        {
            if (data.X != x || data.Y != y)
            {
                return false;
            }

            return true; // CoordinatesMatch(data.customData?.coordinates, coordinates);
        }

        private static bool CoordinatesMatch(float[]? left, float[]? right)
        {
            if (left == null || right == null)
            {
                return left == null && right == null;
            }

            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (!ApproximatelyEqual(left[i], right[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<BpmData> BuildBpmData(float baseBpm, List<BpmEvent> bpmEvents, float songDuration, int songFrequency)
        {
            var normalizedSongDuration = Math.Max(songDuration, 0);
            var orderedEvents = (bpmEvents ?? new List<BpmEvent>())
                .Where(e => e.Bpm > 0)
                .OrderBy(e => e.Seconds)
                .ThenBy(e => e.Beats)
                .ToList();

            var segments = new List<(float StartBeat, float StartSeconds, float Bpm)>
            {
                (0, 0, baseBpm > 0 ? baseBpm : 120)
            };

            foreach (var bpmEvent in orderedEvents)
            {
                if (bpmEvent.Seconds <= 0)
                {
                    segments[0] = (bpmEvent.Beats, 0, bpmEvent.Bpm);
                    continue;
                }

                if (bpmEvent.Seconds >= normalizedSongDuration)
                {
                    continue;
                }

                if (ApproximatelyEqual(bpmEvent.Seconds, segments[segments.Count - 1].StartSeconds))
                {
                    segments[segments.Count - 1] = (bpmEvent.Beats, bpmEvent.Seconds, bpmEvent.Bpm);
                }
                else
                {
                    segments.Add((bpmEvent.Beats, bpmEvent.Seconds, bpmEvent.Bpm));
                }
            }

            var result = new List<BpmData>();
            for (int i = 0; i < segments.Count; i++)
            {
                var current = segments[i];
                var endSeconds = i + 1 < segments.Count ? Math.Min(normalizedSongDuration, segments[i + 1].StartSeconds) : normalizedSongDuration;
                if (endSeconds <= current.StartSeconds)
                {
                    continue;
                }

                var endBeat = i + 1 < segments.Count
                    ? segments[i + 1].StartBeat
                    : current.StartBeat + ((endSeconds - current.StartSeconds) * current.Bpm) / 60f;

                result.Add(new BpmData
                {
                    startSampleIndex = Math.Max(0, (int)Math.Round(current.StartSeconds * songFrequency)),
                    endSampleIndex = Math.Max(0, (int)Math.Round(endSeconds * songFrequency)),
                    startBeat = current.StartBeat,
                    endBeat = endBeat
                });
            }

            if (!result.Any())
            {
                result.Add(new BpmData
                {
                    startSampleIndex = 0,
                    endSampleIndex = Math.Max(0, (int)Math.Round(normalizedSongDuration * songFrequency)),
                    startBeat = 0,
                    endBeat = normalizedSongDuration * (segments[0].Bpm / 60f)
                });
            }

            return result;
        }

        private static float GetSongDuration(BeatmapV3 beatmap)
        {
            if (beatmap.SongLength > 0)
            {
                return (float)beatmap.SongLength;
            }

            var maxSeconds = beatmap.Difficulties
                .SelectMany(d => GetTimedObjects(d.Data))
                .DefaultIfEmpty(0)
                .Max();

            return maxSeconds;
        }

        private static IEnumerable<float> GetTimedObjects(DifficultyV3 diff)
        {
            foreach (var note in diff.Notes)
            {
                yield return note.Seconds;
            }
            foreach (var bomb in diff.Bombs)
            {
                yield return bomb.Seconds;
            }
            foreach (var wall in diff.Walls)
            {
                yield return wall.Seconds + wall.DurationInSeconds;
            }
            foreach (var arc in diff.Arcs)
            {
                yield return Math.Max(arc.Seconds, arc.TailInSeconds);
            }
            foreach (var chain in diff.Chains)
            {
                yield return Math.Max(chain.Seconds, chain.TailInSeconds);
            }
            foreach (var rotation in diff.Rotations)
            {
                yield return rotation.Seconds;
            }
            foreach (var light in diff.Lights)
            {
                yield return light.Seconds;
            }
            foreach (var colorBoost in diff.colorBoostBeatmapEvents)
            {
                yield return colorBoost.Seconds;
            }
            foreach (var bpmEvent in diff.bpmEvents)
            {
                yield return bpmEvent.Seconds;
            }
            foreach (var njsEvent in diff.njsEvents)
            {
                yield return njsEvent.Seconds;
            }
        }

        private static string GetInfoVersion(Info info)
        {
            if (!string.IsNullOrWhiteSpace(info._version) && info._version.StartsWith("4.", StringComparison.Ordinal))
            {
                return info._version;
            }

            return "4.0.0";
        }

        private static string[] GetEnvironmentNames(Info info)
        {
            var names = new List<string>();

            if (info._environmentNames != null)
            {
                names.AddRange(info._environmentNames.OfType<string>().Where(name => !string.IsNullOrWhiteSpace(name)));
            }

            if (!string.IsNullOrWhiteSpace(info._environmentName) && !names.Contains(info._environmentName))
            {
                names.Add(info._environmentName);
            }

            if (!string.IsNullOrWhiteSpace(info._allDirectionsEnvironmentName) && !names.Contains(info._allDirectionsEnvironmentName))
            {
                names.Add(info._allDirectionsEnvironmentName);
            }

            return names.ToArray();
        }

        private static ColorScheme[] GetColorSchemes(Info info)
        {
            return (info._colorSchemes ?? Array.Empty<object>())
                .OfType<ColorScheme>()
                .ToArray();
        }

        private static BeatmapAuthors CreateBeatmapAuthors(string? levelAuthorName)
        {
            var authors = SplitAuthors(levelAuthorName);

            return new BeatmapAuthors
            {
                mappers = authors,
                lighters = authors
            };
        }

        private static string[] SplitAuthors(string? authors)
        {
            if (string.IsNullOrWhiteSpace(authors))
            {
                return Array.Empty<string>();
            }

            return authors
                .Split(',')
                .Select(author => author.Trim())
                .Where(author => !string.IsNullOrWhiteSpace(author))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }

        private static string GetBeatmapFilename(DifficultySet difficultySet)
        {
            if (!string.IsNullOrWhiteSpace(difficultySet.BeatMap?._beatmapFilename))
            {
                return difficultySet.BeatMap._beatmapFilename;
            }

            return $"{difficultySet.Difficulty}.dat";
        }

        private static string GetLightshowFilename(DifficultySet difficultySet, string beatmapFilename)
        {
            if (!string.IsNullOrWhiteSpace(difficultySet.BeatMap?._lightshowDataFilename))
            {
                return difficultySet.BeatMap._lightshowDataFilename;
            }

            return $"{Path.GetFileNameWithoutExtension(beatmapFilename)}.lightshow.dat";
        }

        private static int GetDifficultyRank(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => 1,
                "Normal" => 3,
                "Hard" => 5,
                "Expert" => 7,
                "ExpertPlus" => 9,
                _ => 1
            };
        }

        private static bool ApproximatelyEqual(float left, float right, float epsilon = 0.001f)
        {
            return Math.Abs(left - right) <= epsilon;
        }
    }
}
