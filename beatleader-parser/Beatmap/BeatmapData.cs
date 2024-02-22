using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class BeatmapData
    {
        public BeatmapData(BeatmapInfo info, List<BeatmapDifficultyEntry> diffs)
        {
            Info = info;
            Difficulties = diffs;
        }

        public BeatmapInfo Info { get; set; }
        public List<BeatmapDifficultyEntry> Difficulties { get; set; }
        public float SongLength { get; set; }
    }
}
