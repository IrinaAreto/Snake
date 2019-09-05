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
        private Field _field;
        private int _speed;

        public Game()
        {
            _field = new Field(30, 70);
            ConfigConsole();
            _snake = new Snake();
            _apple = new Apple();
            AppleCors();
            _timer = new Timer(lap);
            _timer.Elapsed += Lap;
            _timer.Enabled = true;
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
            if (Equals(_snake._cors.Last(), _apple.Cors))
            {
                Grow();
                AppleCors();
                ShowApple();
                SetSpeed(++_speed);
            }
            else
            {
                OnItself();
            }
        }

        public void CheckPressedKey()
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
            Console.CursorLeft = _apple.Cors.X;
            Console.CursorTop = _apple.Cors.Y;
            Console.Write("@");
        }

        public void AppleCors()
        {
            var rand = new Random();
            _apple.Cors = new Coordinat()
                {Y = rand.Next(_field.Top, _field.Down), X = rand.Next(_field.Left, _field.Right)};
        }

        public void Grow()
        {
            _snake.Grow(_apple.Cors);
        }

        public void OnItself()
        {
            for (int i = 0; i < _snake._cors.Count - 1; i++)
            {
                if (_snake._cors.Last().Equals(_snake._cors[i]))
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

        public void ConfigConsole()
        {
            Console.CursorVisible = false;
            Console.WindowHeight = _field.Height;
            Console.WindowWidth = _field.Width;
            Console.SetBufferSize(_field.Width, _field.Height);
            Console.SetWindowSize(_field.Width, _field.Height);
        }

        public void SetSpeed(int speed = 0)
        {
            _timer.Interval = GetDelay(speed);
        }

        private double GetDelay(int speed)
        {
            return  lap / speed;
        }
        
    }
}