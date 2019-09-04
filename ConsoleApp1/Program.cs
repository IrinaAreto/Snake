using System;
using Objects;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();

            while (true)
            {
                game.CheckPresssedKey();
            }
        }
    }
}