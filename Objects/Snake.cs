using System;
using System.Collections.Generic;
using System.Linq;

namespace Objects
{
    public class Snake
    {
        private bool _growing;
        public Coordinat Head => Cors.Last();

        public bool OnSelf(Coordinat coors)
        {
            foreach(var item in Cors)
            {
                if (coors.Equals(item))
                    return true;
            }

            return false;
        }
        
        public bool OnItself
        {
            get
            {
                for (int i = 0; i < Cors.Count - 1; i++)
                {
                    if (Head.Equals(Cors[i]))
                        return true;
                }
                return false;
            }
        }

        public List<Coordinat> Cors { get; private set; }
        private Direction _oldDirection = Direction.Right;

        public Snake(int startX)
        {
            Cors = new List<Coordinat>();
            for (int i = startX; i < startX + 4; i++)
            {
                Cors.Add(new Coordinat() {X = i, Y = 8});
            }
        }

        public void Move(Direction direction = Direction.Undefined)
        {
            if (direction != Direction.Undefined &&
                !IsDirectionReverse(direction))
            {
                _oldDirection = direction;
            }

            var offset = GetOffset();
            var last = Cors.Last();
            Cors.Add(new Coordinat()
            {
                X = last.X + offset.X,
                Y = last.Y + offset.Y
            });

            if (_growing)
                _growing = false;
            else
                Cors.RemoveAt(0);
        }

        public void Grow()
        {
            _growing = true;
        }

        public Coordinat GetOffset()
        {
            var cors = new Coordinat();
            if (_oldDirection == Direction.Down)
            {
                cors.Y = 1;
            }
            else if (_oldDirection == Direction.Up)
            {
                cors.Y = -1;
            }
            else if (_oldDirection == Direction.Right)
            {
                cors.X = 1;
            }
            else if (_oldDirection == Direction.Left)
            {
                cors.X = -1;
            }

            return cors;
        }

        private bool IsDirectionReverse(Direction direction)
        {
            if (_oldDirection == Direction.Left && direction == Direction.Right || _oldDirection == Direction.Right && direction == Direction.Left)
            {
                return true;
            }
            else if (_oldDirection == Direction.Up && direction == Direction.Down || _oldDirection == Direction.Down && direction == Direction.Up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}