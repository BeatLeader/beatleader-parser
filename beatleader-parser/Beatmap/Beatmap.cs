using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class Beatmap
    {
        public BeatmapInfo Info { get; set; }
        public List<DifficultyBeatmapSet> DifficultyBeatmapSets { get; set; }
        public float SongLength { get; set; }
    }
}
