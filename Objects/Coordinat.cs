using System;

namespace Objects
{
    public class Coordinat
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinat() {}

        public Coordinat(Field field)
        {
            var rand = new Random();
            Y = rand.Next(field.Top + 1, field.Down - 3);
            X = rand.Next(field.Left + 1, field.Right - 1);
        }
        
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

        public override int GetHashCode()
        {
            return X + Y;
        }
    }
}