using System.Collections.Generic;

namespace Objects
{
    public class Apple
    {
        public Coordinat Cors { get; set; }
        public List<Coordinat> _apples;
        public List<Coordinat> _poisonApples;
        public Apple()
        {
            _apples = new List<Coordinat>();
            _poisonApples = new List<Coordinat>();
        }
    }
}