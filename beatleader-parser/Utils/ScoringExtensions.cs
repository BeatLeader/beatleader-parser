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
    public enum NoteScoreDefinition
    {
        NoScore,
        Normal,
        SliderHead,
        SliderTail,
        BurstSliderHead,
        BurstSliderElement
    }

    public static class NoteScoreDefinitionExtensions
    {
        public static int MaxCutScore(this NoteScoreDefinition definition)
        {
            switch (definition)
            {
                case NoteScoreDefinition.NoScore:
                    return 0;
                case NoteScoreDefinition.Normal:
                case NoteScoreDefinition.SliderHead:
                case NoteScoreDefinition.SliderTail:
                    return NoteScoreDefaults.MaxCenterDistanceCutScore + NoteScoreDefaults.MaxBeforeCutScore + NoteScoreDefaults.MaxAfterCutScore;
                case NoteScoreDefinition.BurstSliderHead:
                    return NoteScoreDefaults.MaxCenterDistanceCutScore + NoteScoreDefaults.MaxBeforeCutScore;
                case NoteScoreDefinition.BurstSliderElement:
                    return 20;
                default:
                    throw new ArgumentOutOfRangeException(nameof(definition), "Unknown NoteScoreDefinition");
            }
        }
    }
    public static class NoteScoreDefaults
    {
        public const int MaxBeforeCutScore = 70;
        public const int MaxCenterDistanceCutScore = 15;
        public const int MaxAfterCutScore = 30;
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

                NoteScoreDefinition type;
                if (matchesTail)
                {
                    type = NoteScoreDefinition.SliderTail;
                } else if (matchesBurst)
                {
                    type = NoteScoreDefinition.BurstSliderHead;
                } else if (matchesHead)
                {
                    type = NoteScoreDefinition.SliderHead;
                } else
                {
                    type = NoteScoreDefinition.Normal;
                }

                return new MaxScoreCounterElement(type, note.Seconds);
            }).ToList();

            var burstItems = burstSliders.SelectMany(bs =>
            {
                var sliceCount = bs.SliceCount;
                return sliceCount == 0 ? new List<MaxScoreCounterElement>() : Enumerable.Range(1, sliceCount - 1).Select(i =>
                {
                    float t = (float)i / (sliceCount - 1);
                    var beat = bs.Seconds + (bs.TailInSeconds - bs.Seconds) * t;
                    return new MaxScoreCounterElement(NoteScoreDefinition.BurstSliderElement, beat);
                });
            }).ToList();

            var items = noteItems.Concat(burstItems)
                .OrderBy(elem => elem.Time)
                .ThenBy(elem => elem.ScoreDef.MaxCutScore())
                .ToList();

            var maxScores = new List<(float, int)>();
            var smc = new ScoreMultiplierCounter();
            var score = 0;
            foreach (var item in items) {
                smc = smc.ProcessMultiplierEvent(MultiplierEventType.Positive);
                score += item.ScoreDef.MaxCutScore() * smc.Multiplier;

                maxScores.Add((item.Time, score));
            }

            return maxScores;
        }

        public static int MaxScore(this DifficultySet self) {
            var graph = MaxScoreGraph(self);
            return graph.Count > 0 ? graph.Last().Item2 : 0;
        }
    }
}
