using System.Collections.Generic;

namespace Parser.Map.Difficulty.V3.Custom
{
    public class Customdata
    {
        public float time { get; set; }
        public List<Bookmark> bookmarks { get; set; }
        public bool bookmarksUseOfficialBpmEvents { get; set; }
        public List<Environment> environment { get; set; }
    }
}
