using System;
using System.Linq;
using System.Timers;

namespace Objects
{
    public class Game
    {
        private Snake _snake;
        private Apple _apple;
        private readonly Timer _timer;
        private double lap = 500;
        private Direction _direction = Direction.Undefined;

        public Game()
        {
            ConsoleSize();
            _snake = new Snake();
            _apple = new Apple();
            AppleCors();
            _timer = new Timer(lap);
            _timer.Elapsed += Lap;
            _timer.Enabled = true;
            Console.CursorVisible = false;
            Render();
        }

        ~Game()
        {
            _timer.Elapsed -= Lap;
        }

        private void Lap(object o, ElapsedEventArgs e)
        {
            _snake.Move(_direction);
            Render();
            if (Equals(_snake._cors.Last(), _apple._cors))
            {
                Grow();
                AppleCors();
                ShowApple();
            }
            MoveToItself();
        }

        public void CheckPresssedKey()
        {
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    _direction = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    _direction = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    _direction = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    _direction = Direction.Right;
                    break;
            }
        }

        public void Render()
        {
            Console.Clear();
            ShowApple();
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var item in _snake._cors)
            {
                Console.CursorLeft = item.X;
                Console.CursorTop = item.Y;
                Console.Write("#");
            }
        }

        public void ShowApple()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.CursorLeft = _apple._cors.X;
            Console.CursorTop = _apple._cors.Y;
            Console.Write("@");
        }

        public void AppleCors()
        {
            var rand = new Random();
            _apple._cors = new Coordinat() { X = rand.Next(0, 60), Y = rand.Next(0, 40) };
        }

        public void Grow()
        {
            _snake.Grow(_apple._cors.X, _apple._cors.Y);
        }
        public void MoveToItself()
        {
            foreach (var item in _snake._cors)
            {
                if (Equals(item, _snake._cors.Last()))
                {
                    _timer.Enabled = false;
                    GameOver();
                }
            }
        }
        public void GameOver()
        {
            Console.Clear();
            var message = "Game over!";
            Console.CursorLeft = (Console.BufferWidth - message.ToString().Length) / 2;
            Console.CursorTop = 10;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void ConsoleSize()
        {
            Console.SetWindowSize(70, 45);
            Console.SetBufferSize(70, 45);
        }
    }
}