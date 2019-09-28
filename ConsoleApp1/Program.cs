using Objects;

namespace ConsoleApp1
{
    static class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            
            while (true)
            {
                game.CheckPressedKey();
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}