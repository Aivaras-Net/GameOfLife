using GameOfLife.Infrastructure;
using GameOfLife.Interfaces;

namespace GameOfLife
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRenderer renderer = new ConsoleRenderer();
            IGameLogic gameLogic = new GameLogic();
            IInputHandler inputHandler = new UserInputHandler();
            Game game = new Game(renderer, gameLogic, inputHandler);
            game.Start();
        }
    }
}
