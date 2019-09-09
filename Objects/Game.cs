using System;
using System.Linq;
using System.Runtime.InteropServices;
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
        private int _speed = 5;
        private int _score = 0;
        private int _quantity = 1;

        public Game()
        {
            _field = new Field(30, 70);
            ConfigConsole();
            _snake = new Snake();
            _apple = new Apple();
            _quantity = SetQuantity();
            AppleCors(_quantity);
            _timer = new Timer();
            SetSpeed(_speed);
            _timer.Elapsed += Lap;
            _timer.Enabled = true;
            Render();
            ShowBorder();
            ShowScore();
        }
        ~Game()
        {
            _timer.Elapsed -= Lap;
        }

        private void Lap(object o, ElapsedEventArgs e)
        {
            PrintSymbol(_snake._cors.First().X, _snake._cors.First().Y, ' ');
            _snake.Move(_direction);
            CheckDead();
            CheckPoisonApple();

            Render();
            if (_apple._apples.Count == 0)
            {
                _quantity = SetQuantity();
                AppleCors(_quantity);
                ShowApple();
            }
            else
            {
                foreach (var item in _apple._apples)
                {
                    if (_snake._cors.Last().Equals(item))
                    {
                        _snake.Grow(item);
                        RemuveApple(item);
                        SetSpeed(++_speed);
                        _score++;
                        ShowScore();
                        Win();
                        PoisonAppleCors();
                        ShowPoisonApple();
                        return;
                    }
                    else
                    {
                        OnItself();
                    }
                }
            }
        }

        private void CheckDead()
        {
            var head = _snake._cors.Last();
            if (head.X == _field.Left || head.X == _field.Right - 1 ||
                head.Y == _field.Top || head.Y == _field.Down - 3)
            {
                GameOver();
            }
        }

        private void CheckPoisonApple()
        {
            foreach (var item in _apple._poisonApples)
            {
                if (_snake._cors.Last().Equals(item))
                {
                    GameOver();
                }
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
            ShowApple();
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var item in _snake._cors)
            {
                Console.CursorLeft = item.X;
                Console.CursorTop = item.Y;
                Console.Write("#");
            }
        }

        public int SetQuantity()
        {
            var rand = new Random();
            return rand.Next(1, 4);
        }

        public void ShowApple()
        {
            foreach (var item in _apple._apples)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.CursorLeft = item.X;
                Console.CursorTop = item.Y;
                Console.Write("@");
            }
        }

        public void AppleCors(int quantity)
        {
            var rand = new Random();
            for (int i = 0; i < quantity; i++)
            {
                _apple._apples.Add(new Coordinat()
                { Y = rand.Next(_field.Top + 1, _field.Down - 3), X = rand.Next(_field.Left + 1, _field.Right - 1) });
            }
        }

        public void RemuveApple(Coordinat coordinat)
        {
            _apple._apples.Remove(coordinat);
        }

        public void PoisonAppleCors()
        {
            var rand = new Random();
            _apple._poisonApples.Add(new Coordinat()
            { Y = rand.Next(_field.Top + 1, _field.Down - 3), X = rand.Next(_field.Left + 1, _field.Right - 1) });
        }

        public void ShowPoisonApple()
        {
            foreach(var item in _apple._poisonApples)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.CursorLeft = item.X;
                Console.CursorTop = item.Y;
                Console.Write("¸");
            }
        }

        public void OnItself()
        {
            for (int i = 0; i < _snake._cors.Count - 1; i++)
            {
                if (_snake._cors.Last().Equals(_snake._cors[i]))
                    GameOver();
            }
        }

        public void GameOver()
        {
            _timer.Enabled = false;
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

        public void SetSpeed(int speed)
        {
            _timer.Interval = GetDelay(speed);
        }

        private double GetDelay(int speed)
        {
            return lap / speed * 2 + lap / 2;
        }

        private void PrintSymbol(int x, int y, char symbol, ConsoleColor background = ConsoleColor.Black, ConsoleColor font = ConsoleColor.White)
        {
            Console.CursorTop = y;
            Console.CursorLeft = x;
            Console.ForegroundColor = font;
            Console.BackgroundColor = background;
            Console.Write(symbol);
        }

        public void ShowBorder()
        {
            for (int i = 0; i < _field.Height - 1; i++)
            {
                PrintSymbol(0, i, '/');
                PrintSymbol(_field.Width - 1, i, '/');
            }
            for (int i = 0; i < _field.Width - 1; i++)
            {
                PrintSymbol(i, 0, '^');
                PrintSymbol(i, _field.Height - 3, '*');
            }
        }

        public void ShowScore()
        {
            Console.CursorTop = _field.Height - 2;
            Console.CursorLeft = _field.Width - 12;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Score: " + _score);
        }

        public void Win()
        {
            if (_score == 25)
            {
                _timer.Enabled = false;
                Console.Clear();
                var message = "YOU WIN!";
                Console.CursorLeft = (Console.BufferWidth - message.ToString().Length) / 2;
                Console.CursorTop = 10;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
            }
        }

        public void ChooseSpeed(int speed)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Add)
            {
                SetSpeed(++speed);
            }
        }
    }
}