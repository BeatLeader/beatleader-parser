using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class BeatmapDifficultyEntry
    {
        public Difficulty Difficulty { get; set; }
        public int DifficultyRank { get; set; }
        public BeatmapDifficulty BeatmapDifficulty { get; set; }
    }
}
