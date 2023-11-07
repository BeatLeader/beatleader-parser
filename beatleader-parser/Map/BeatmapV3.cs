using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;

namespace Parser.Map
{
    public class BeatmapV3
    {
        internal BeatmapV3() { }

        public Info Info { get; set; }
        public List<DifficultySet> Difficulties { get; set; } = new();

        public double SongLength { get; set; }
    }

    public class SingleDiffBeatmapV3
    {
        internal SingleDiffBeatmapV3() { }

        public Info Info { get; set; }
        public DifficultySet Difficulty { get; set; }

        public double SongLength { get; set; }
    }

    public class DifficultySet
    {
        public string Difficulty { get; set; }
        public string Characteristic { get; set; }
        public DifficultyV3 Data { get; set; }
        
        internal DifficultySet(string difficulty, string characteristic, DifficultyV3 data)
        {
            Difficulty = difficulty;
            Characteristic = characteristic;
            Data = data;
        }
    }
}
