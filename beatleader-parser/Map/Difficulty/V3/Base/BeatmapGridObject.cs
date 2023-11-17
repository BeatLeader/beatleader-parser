namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapGridObject : BeatmapObject
    {
        public int x { get; set; }
        public int y { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            BeatmapGridObject otherObject = (BeatmapGridObject)obj;
            return Equals(Beats, otherObject.Beats) &&
                   Equals(x, otherObject.x) &&
                   Equals(y, otherObject.y);
        }
    }
}
