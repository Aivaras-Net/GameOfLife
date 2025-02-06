using GameOfLife.CLI.Infrastructure;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Infrastucture;

namespace GameOfLife.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRenderer renderer = new ConsoleRenderer();
            IGameLogic gameLogic = new GameLogic();
            IInputHandler inputHandler = new FieldInputHandler();
            IGameFieldAnalyzer gameFieldAnalyzer = new GameFieldAnalyzer();
            IGameInputHandler gameInputHandler = new GameInputHandler();
            IFileManager gameFileManager = new FileManager();
            GameManager game = new GameManager(renderer, gameLogic, inputHandler, gameFieldAnalyzer, gameFileManager, gameInputHandler);
            game.Start();
        }
    }
}
