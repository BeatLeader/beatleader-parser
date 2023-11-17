namespace Parser.Map.Difficulty.V3.Event
{
    public class Customdata
    {
        public float[] color { get; set; }
        public float rotation { get; set; }
        public float prop { get; set; }
        public float speed { get; set; }
        public float step { get; set; }
        public int[] lightID { get; set; }
        public int direction { get; set; }
        public bool lockRotation { get; set; }
        public string lerpType { get; set; }
        public string easing { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Customdata otherCustomdata = (Customdata)obj;
            return Equals(color, otherCustomdata.color) &&
                   Equals(rotation, otherCustomdata.rotation) &&
                   Equals(prop, otherCustomdata.prop) &&
                   Equals(speed, otherCustomdata.speed) &&
                   Equals(step, otherCustomdata.step) &&
                   Equals(lightID, otherCustomdata.lightID) &&
                   Equals(direction, otherCustomdata.direction) &&
                   Equals(lockRotation, otherCustomdata.lockRotation) &&
                   Equals(lerpType, otherCustomdata.lerpType) &&
                   Equals(easing, otherCustomdata.easing);
        }   
    }
}
