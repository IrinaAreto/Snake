using System;
using System.Collections.Generic;
using System.Linq;

namespace Objects
{
    public class Snake
    {
        public List<Coordinat> _cors;
        private Direction _oldDirection = Direction.Right;

        public Snake()
        {
            _cors = new List<Coordinat>();
            for (int i = 0; i < 4; i++)
            {
                _cors.Add(new Coordinat() {X = i, Y = 0});
            }
        }

        public void Move(Direction direction = Direction.Undefined)
        {
            if (direction != Direction.Undefined)
            {
                _oldDirection = direction;
            }

            var offset = GetOffset();
            var last = _cors.Last();
            _cors.Add(new Coordinat()
            {
                X = last.X + offset.X,
                Y = last.Y + offset.Y
            });

            _cors.RemoveAt(0);
        }

        public void Grow(Coordinat coor)
        {
            _cors.Add(coor);
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
    }
}