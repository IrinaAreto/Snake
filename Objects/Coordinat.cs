namespace Objects
{
    public class Coordinat
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (base.Equals(obj))
                return true;

            if (obj is Coordinat coordinat)
            {
                if (coordinat.X == this.X && coordinat.Y == this.Y)
                    return true;
            }
            
            return false;
        }
    }
}