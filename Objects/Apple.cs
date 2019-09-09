using System.Collections.Generic;

namespace Objects
{
    public class Apple
    {
        public Coordinat Cors { get; set; }
        public List<Coordinat> _apples;
        public Apple()
        {
            _apples = new List<Coordinat>();
        }
    }
}