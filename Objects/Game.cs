using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;

namespace Objects
{
    public class Game
    {
        private Snake _snake;
        private List<Apple> _apples;
        private readonly Timer _timer;
        private double lap = 500;
        private Direction _direction = Direction.Undefined;
        private Field _field;
        private int _speed = 5;
        private int _score = 0;
        private int _quantity = 1;
        private bool _gameOver = false;

        public Game()
        {
            _field = new Field(30, 70);
            ConfigConsole();
            _timer = new Timer();
            _timer.Elapsed += Lap;
            Init();
        }

        ~Game()
        {
            _timer.Elapsed -= Lap;
        }

        private void Init()
        {
            _snake = new Snake();
            _apples = new List<Apple>();
            _speed = 5;
            _score = 0;
            _quantity = 1;
            _gameOver = false;
            _quantity = GetQuantity();
            AddApple(_quantity);
            SetSpeed(_speed);
            _timer.Enabled = true;
            Render();
            ShowBorder();
            ShowScore();
        }

        private void Lap(object o, ElapsedEventArgs e)
        {
            ClearSnakeTail();
            _snake.Move(_direction);

            Render();
            OnApple();

            CheckBorder();

            if (_snake.OnItself)
            {
                GameOver();
            }
        }

        private void OnApple()
        {
            foreach (var item in _apples)
            {
                if (_snake.Head.Equals(item.Cors))
                {
                    if (item.Type == AppleType.GrowSize)
                    {
                        if (++_score == 25)
                        {
                            Win();
                        }

                        ShowScore();

                        RemoveApple(item.Cors);
                        AddApple(1, AppleType.Poison);

                        _snake.Grow();
                        SetSpeed(++_speed);

                        if (_apples.All(a => a.Type != AppleType.GrowSize))
                        {
                            _quantity = GetQuantity();
                            AddApple(_quantity);
                            ShowApple();
                        }
                    }
                    else if (item.Type == AppleType.Poison)
                    {
                        GameOver();
                    }

                    return;
                }
            }
        }

        private void ClearSnakeTail()
        {
            PrintSymbol(_snake.Cors.First().X, _snake.Cors.First().Y, ' ');
        }

        private void CheckBorder()
        {
            var head = _snake.Cors.Last();
            if (head.X == _field.Left || head.X == _field.Right - 1 ||
                head.Y == _field.Top || head.Y == _field.Down - 3)
            {
                GameOver();
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
                case ConsoleKey.Add:
                    SetSpeed(++_speed);
                    break;
                case ConsoleKey.Subtract:
                    SetSpeed(--_speed);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.Spacebar:
                    PauseGame();
                    if (_gameOver)
                    {
                        Console.Clear();
                        PlayAgain();
                    }

                    break;
            }
        }

        private void PauseGame()
        {
            if (_timer.Enabled)
            {
                _timer.Enabled = false;
                Console.Clear();
                var message = "  Pause  ";
                Console.CursorLeft = (Console.BufferWidth - message.ToString().Length) / 2;
                Console.CursorTop = 10;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                if (!_gameOver)
                {
                    Console.Clear();
                    Lap(this, null);
                    ShowBorder();
                    ShowScore();
                    _timer.Enabled = true;
                }
            }
        }

        private void Render()
        {
            ShowApple();
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var item in _snake.Cors)
            {
                Console.CursorLeft = item.X;
                Console.CursorTop = item.Y;
                Console.Write("#");
            }
        }

        private int GetQuantity()
        {
            var rand = new Random();
            return rand.Next(1, 4);
        }

        private void ShowApple()
        {
            foreach (var item in _apples)
            {
                var color = ConsoleColor.Green;
                var backColor = ConsoleColor.Black;
                var symbol = '@';

                switch (item.Type)
                {
                    case AppleType.GrowSize:
                        color = ConsoleColor.Green;
                        backColor = ConsoleColor.DarkGreen;
                        symbol = '@';
                        break;
                    case AppleType.SpeedUp:
                        color = ConsoleColor.Yellow;
                        backColor = ConsoleColor.Cyan;
                        symbol = '$';
                        break;
                    case AppleType.SpeedDown:
                        color = ConsoleColor.Cyan;
                        backColor = ConsoleColor.Yellow;
                        symbol = '$';
                        break;
                    case AppleType.Poison:
                        color = ConsoleColor.Red;
                        backColor = ConsoleColor.DarkRed;
                        symbol = 'ё';
                        break;
                    default:
                        color = ConsoleColor.Black;
                        backColor = ConsoleColor.Black;
                        symbol = ' ';
                        break;
                }

                PrintSymbol(item.Cors, symbol, backColor, color);
            }
        }

        private void AddApple(int quantity, AppleType type = AppleType.GrowSize)
        {
            var rand = new Random();
            for (int i = 0; i < quantity; i++)
            {
                var cors = new Coordinat(_field);

                while (_snake.OnSelf(cors))
                {
                    cors = new Coordinat(_field);
                }

                _apples.Add(new Apple(cors, type));
            }
        }

        private void RemoveApple(Coordinat coordinat)
        {
            _apples.RemoveAll(a => a.Cors.Equals(coordinat));
        }

        private void GameOver()
        {
            _timer.Enabled = false;
            Console.Clear();
            _gameOver = true;
            var message = "Game over!";
            var message2 = "To play again press space-key";
            Console.CursorLeft = (Console.BufferWidth - message.ToString().Length) / 2;
            Console.CursorTop = 10;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorLeft = (Console.BufferWidth - message2.ToString().Length) / 2;
            Console.CursorTop = 14;
            Console.WriteLine(message2);
        }

        private void PlayAgain()
        {
            Init();
        }

        private void ConfigConsole()
        {
            Console.CursorVisible = false;
            Console.WindowHeight = _field.Height;
            Console.WindowWidth = _field.Width;
            Console.SetBufferSize(_field.Width, _field.Height);
            Console.SetWindowSize(_field.Width, _field.Height);
        }

        private void SetSpeed(int speed)
        {
            _timer.Interval = GetDelay(speed);
        }

        private double GetDelay(int speed)
        {
            return lap / speed * 2 + lap / 2;
        }

        private void PrintSymbol(Coordinat coordinat, char symbol, ConsoleColor background = ConsoleColor.Black,
            ConsoleColor font = ConsoleColor.White)
            => PrintSymbol(coordinat.X, coordinat.Y, symbol, background, font);

        private void PrintSymbol(int x, int y, char symbol, ConsoleColor background = ConsoleColor.Black,
            ConsoleColor font = ConsoleColor.White)
        {
            var originFontColor = Console.ForegroundColor;
            var originBackgroundColor = Console.BackgroundColor;

            Console.CursorTop = y;
            Console.CursorLeft = x;
            Console.ForegroundColor = font;
            Console.BackgroundColor = background;
            Console.Write(symbol);

            Console.ForegroundColor = originFontColor;
            Console.BackgroundColor = originBackgroundColor;
        }

        private void ShowBorder()
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

        private void ShowScore()
        {
            Console.CursorTop = _field.Height - 2;
            Console.CursorLeft = _field.Width - 12;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Score: " + _score);
        }

        private void Win()
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
}