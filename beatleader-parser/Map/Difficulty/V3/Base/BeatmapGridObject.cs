namespace Parser.Map.Difficulty.V3.Base
{
    public class BeatmapGridObject : BeatmapObject
    {
        public int x { get; set; }
        public int y { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BeatmapGridObject @object &&
                   base.Equals(obj) &&
                   Beats == @object.Beats &&
                   x == @object.x &&
                   y == @object.y;
        }

        public override int GetHashCode()
        {
            int hashCode = 72470987;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Beats.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
    }
}
