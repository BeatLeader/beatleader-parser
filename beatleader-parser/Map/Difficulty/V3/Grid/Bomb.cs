using Parser.Map.Difficulty.V3.Base;
using System.Drawing;

namespace Parser.Map.Difficulty.V3.Grid
{
    public class Bomb : BeatmapGridObject
    {
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Bomb otherBomb = (Bomb)obj;
            return Equals(Beats, otherBomb.Beats) &&
                   Equals(x, otherBomb.x) &&
                   Equals(y, otherBomb.y);
        }
    }
}
