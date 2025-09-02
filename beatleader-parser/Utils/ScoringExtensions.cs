using Parser.Map;
using System;
using System.Collections.Generic;
using System.Linq;

// Copied from: https://github.com/beatmaps-io/beatsaver-common-mp/blob/ccfc56bffd02dc2b08ce285eeb9be9de8646b2ba/src/commonMain/kotlin/io/beatmaps/common/beatsaber/maxScore.kt
// Translated from Kotlin to C# with ChatGPT
namespace Parser.Utils
{
    public enum MultiplierEventType
    {
        Positive,
        Neutral,
        Negative
    }

    public enum ScoringType
    {
        Ignore = -1,
        NoScore = 0,
        Normal = 1,
        ArcHead = 2,
        ArcTail = 3,
        ChainHead = 4,
        ChainLink = 5,
        ArcHeadArcTail = 6,
        ChainHeadArcTail = 7,
        ChainLinkArcHead = 8,
        ChainHeadArcHead = 9,
        ChainHeadArcHeadArcTail = 10
    }

    public class NoteScoreDefinition
    {
        public readonly int maxCenterDistanceCutScore;
        public readonly int minBeforeCutScore;
        public readonly int maxBeforeCutScore;
        public readonly int minAfterCutScore;
        public readonly int maxAfterCutScore;
        public readonly int fixedCutScore;

        public int maxCutScore => this.maxCenterDistanceCutScore + this.maxBeforeCutScore + this.maxAfterCutScore + this.fixedCutScore;

        public int executionOrder => this.maxCutScore;

        public NoteScoreDefinition(
            int maxCenterDistanceCutScore,
            int minBeforeCutScore,
            int maxBeforeCutScore,
            int minAfterCutScore,
            int maxAfterCutScore,
            int fixedCutScore)
        {
            this.maxCenterDistanceCutScore = maxCenterDistanceCutScore;
            this.minBeforeCutScore = minBeforeCutScore;
            this.maxBeforeCutScore = maxBeforeCutScore;
            this.minAfterCutScore = minAfterCutScore;
            this.maxAfterCutScore = maxAfterCutScore;
            this.fixedCutScore = fixedCutScore;
        }
    }

    public class MaxScoreCounterElement
    {
        public NoteScoreDefinition ScoreDef { get; }
        public float Time { get; }

        public MaxScoreCounterElement(NoteScoreDefinition scoreDef, float time)
        {
            ScoreDef = scoreDef;
            Time = time;
        }
    }

    public class ScoreMultiplierCounter
    {
        public int Multiplier { get; }
        public int MultiplierIncreaseProgress { get; }
        public int MultiplierIncreaseMaxProgress { get; }

        public ScoreMultiplierCounter(int multiplier = 1, int multiplierIncreaseProgress = 0, int multiplierIncreaseMaxProgress = 2)
        {
            Multiplier = multiplier;
            MultiplierIncreaseProgress = multiplierIncreaseProgress;
            MultiplierIncreaseMaxProgress = multiplierIncreaseMaxProgress;
        }

        public float NormalizedProgress() => MultiplierIncreaseProgress / (float)MultiplierIncreaseMaxProgress;

        public ScoreMultiplierCounter ProcessMultiplierEvent(MultiplierEventType type)
        {
            switch (type)
            {
                case MultiplierEventType.Positive:
                    if (Multiplier < 8)
                    {
                        if (MultiplierIncreaseProgress >= MultiplierIncreaseMaxProgress - 1)
                        {
                            return new ScoreMultiplierCounter(Multiplier * 2, 0, Multiplier * 4);
                        } else
                        {
                            return new ScoreMultiplierCounter(Multiplier, MultiplierIncreaseProgress + 1, MultiplierIncreaseMaxProgress);
                        }
                    } else
                    {
                        return this;
                    }
                case MultiplierEventType.Negative:
                    if (MultiplierIncreaseProgress > 0)
                    {
                        return new ScoreMultiplierCounter(Multiplier, 0, MultiplierIncreaseMaxProgress);
                    } else if (Multiplier > 1)
                    {
                        return new ScoreMultiplierCounter(Multiplier / 2, MultiplierIncreaseProgress, Multiplier);
                    } else
                    {
                        return this;
                    }
                case MultiplierEventType.Neutral:
                    return this;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Unknown MultiplierEventType");
            }
        }
    }

    public static class ScoringExtensions
    {
        public static readonly Dictionary<ScoringType, NoteScoreDefinition> ScoreDefinitions = new Dictionary<ScoringType, NoteScoreDefinition>()
        {
            {
                ScoringType.Ignore,
                (NoteScoreDefinition) null
            },
            {
                ScoringType.NoScore,
                new NoteScoreDefinition(0, 0, 0, 0, 0, 0)
            },
            {
                ScoringType.Normal,
                new NoteScoreDefinition(15, 0, 70, 0, 30, 0)
            },
            {
                ScoringType.ArcHead,
                new NoteScoreDefinition(15, 0, 70, 30, 30, 0)
            },
            {
                ScoringType.ArcTail,
                new NoteScoreDefinition(15, 70, 70, 0, 30, 0)
            },
            {
                ScoringType.ChainHead,
                new NoteScoreDefinition(15, 0, 70, 0, 0, 0)
            },
            {
                ScoringType.ChainLink,
                new NoteScoreDefinition(0, 0, 0, 0, 0, 20)
            },
            {
                ScoringType.ArcHeadArcTail,
                new NoteScoreDefinition(15, 70, 70, 30, 30, 0)
            },
            {
                ScoringType.ChainHeadArcTail,
                new NoteScoreDefinition(15, 70, 70, 30, 30, 0)
            },
            {
                ScoringType.ChainLinkArcHead,
                new NoteScoreDefinition(0, 0, 0, 0, 0, 20)
            },
            {
                ScoringType.ChainHeadArcHead,
                new NoteScoreDefinition(15, 0, 70, 30, 30, 0)
            },
            {
                ScoringType.ChainHeadArcHeadArcTail,
                new NoteScoreDefinition(15, 70, 70, 30, 30, 0)
            }
        };

        public static List<(float, int)> MaxScoreGraph(this DifficultySet self)
        {
            var notes = self.Data.Notes.Where(note => note.Color == 0 || note.Color == 1);
            var sliders = self.Data.Arcs;
            var burstSliders = self.Data.Chains;

            var slidersByBeat = sliders.GroupBy(s => s.BpmTime).ToDictionary(g => g.Key, g => g.ToList());
            var slidersByTailBeat = sliders.GroupBy(s => s.TailBpmTime).ToDictionary(g => g.Key, g => g.ToList());
            var burstSlidersByBeat = burstSliders.GroupBy(s => s.BpmTime).ToDictionary(g => g.Key, g => g.ToList());

            var noteItems = notes.Select(note =>
            {
                var matchesHead = slidersByBeat.ContainsKey(note.BpmTime) && slidersByBeat[note.BpmTime].Any(s => note.Color == s.Color && note.x == s.x && note.y == s.y);
                var matchesTail = slidersByTailBeat.ContainsKey(note.BpmTime) && slidersByTailBeat[note.BpmTime].Any(s => note.Color == s.Color && note.x == s.tx && note.y == s.ty);
                var matchesBurst = burstSlidersByBeat.ContainsKey(note.BpmTime) && burstSlidersByBeat[note.BpmTime].Any(s => note.Color == s.Color && note.x == s.x && note.y == note.y);

                if (matchesBurst && matchesHead && matchesTail) {
                    return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ChainHeadArcHeadArcTail], note.Seconds);
                }

                if (matchesBurst && matchesHead) {
                    return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ChainHeadArcHead], note.Seconds);
                }

                if (matchesBurst && matchesTail) {
                    return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ChainHeadArcTail], note.Seconds);
                }

                if (matchesHead && matchesTail) {
                    return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ArcHeadArcTail], note.Seconds);
                }

                if (matchesBurst) {
                    return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ChainHead], note.Seconds);
                }

                if (matchesHead) {
                    return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ArcHead], note.Seconds);
                }

                if (matchesTail) {
                    return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ArcTail], note.Seconds);
                }

                return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.Normal], note.Seconds);
            }).ToList();

            var burstItems = burstSliders.SelectMany(bs =>
            {
                var sliceCount = bs.SliceCount;
                return sliceCount == 0 ? new List<MaxScoreCounterElement>() : Enumerable.Range(1, sliceCount - 1).Select(i =>
                {
                    float t = (float)i / (sliceCount - 1);

                    var seconds = bs.Seconds + (bs.TailInSeconds - bs.Seconds) * t;
                    var beat = bs.Beats + (bs.TailInBeats - bs.Beats) * t;

                    var matchesHead = slidersByBeat.ContainsKey(beat) && slidersByBeat[beat].Any(s => bs.Color == s.Color && bs.x == s.x && bs.y == s.y);

                    if (matchesHead) {
                        return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ChainLinkArcHead], seconds);
                    } else {
                        return new MaxScoreCounterElement(ScoreDefinitions[ScoringType.ChainLink], seconds);
                    }
                });
            }).ToList();

            var items = noteItems.Concat(burstItems)
                .OrderBy(elem => elem.Time)
                .ThenBy(elem => elem.ScoreDef.maxCutScore)
                .ToList();

            var maxScores = new List<(float, int)>();
            var smc = new ScoreMultiplierCounter();
            var score = 0;
            foreach (var item in items) {
                smc = smc.ProcessMultiplierEvent(MultiplierEventType.Positive);
                score += item.ScoreDef.maxCutScore * smc.Multiplier;

                maxScores.Add((item.Time, score));
            }

            return maxScores;
        }

        public static bool IsV3Pepega(this DifficultySet self)
        {
            var notes = self.Data.Notes.Where(note => note.Color == 0 || note.Color == 1);
            var sliders = self.Data.Arcs;
            var burstSliders = self.Data.Chains;

            var slidersByBeat = sliders.GroupBy(s => s.BpmTime).ToDictionary(g => g.Key, g => g.ToList());
            var slidersByTailBeat = sliders.GroupBy(s => s.TailBpmTime).ToDictionary(g => g.Key, g => g.ToList());
            var burstSlidersByBeat = burstSliders.GroupBy(s => s.BpmTime).ToDictionary(g => g.Key, g => g.ToList());

            var noteItems = notes.Any(note =>
            {
                var matchesHead = slidersByBeat.ContainsKey(note.BpmTime) && slidersByBeat[note.BpmTime].Any(s => note.Color == s.Color && note.x == s.x && note.y == s.y);
                var matchesTail = slidersByTailBeat.ContainsKey(note.BpmTime) && slidersByTailBeat[note.BpmTime].Any(s => note.Color == s.Color && note.x == s.tx && note.y == s.ty);
                var matchesBurst = burstSlidersByBeat.ContainsKey(note.BpmTime) && burstSlidersByBeat[note.BpmTime].Any(s => note.Color == s.Color && note.x == s.x && note.y == note.y);

                return matchesBurst && matchesHead && matchesTail || matchesBurst && matchesHead || matchesBurst && matchesTail || matchesHead && matchesTail;
            });

            return noteItems;
        }

        public static int MaxScore(this DifficultySet self) {
            var graph = MaxScoreGraph(self);
            return graph.Count > 0 ? graph.Last().Item2 : 0;
        }
    }
}
