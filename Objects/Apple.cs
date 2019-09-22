using System;

namespace Objects
{
    public class Apple
    {
        public Coordinat Cors { get; private set; }
        public AppleType Type { get; private set; }
        public Apple(Coordinat cors, AppleType type)
        {
            Type = type;
            Cors = cors;
        }
    }

    public enum AppleType
    {
        Undefined,
        GrowSize,
        SpeedUp,
        SpeedDown,
        Poison
    }
}