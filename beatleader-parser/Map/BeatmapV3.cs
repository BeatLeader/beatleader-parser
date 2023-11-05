using Parser.Map.Difficulty.V3.Base;
using System.Collections.Generic;

namespace Parser.Map
{
    public class BeatmapV3
    {
        private static BeatmapV3? _instance;

        private BeatmapV3() { }

        public static BeatmapV3 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BeatmapV3();
                }
                return _instance;
            }
        }

        public Info? Info { get; set; }
        public List<DifficultySet> Difficulties { get; set; } = new();

        public double SongLength { get; set; }

        public static BeatmapV3 Reset()
        {
            _instance = new();
            return _instance;
        }
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
