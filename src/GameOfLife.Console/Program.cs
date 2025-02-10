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
            IGameSetupInputHandler gameSetupInputHandler = new GameSetupInputHandler();
            IGameFieldAnalyzer gameFieldAnalyzer = new GameFieldAnalyzer();
            IGameInputHandler gameInputHandler = new GameInputHandler();
            IFileManager fileManager = new FileManager();
            ISaveFileSelector saveFileSelector = new SaveFileSelector();
            //GameManager game = new GameManager(renderer, gameLogic, gameSetupInputHandler, gameFieldAnalyzer, fileManager, gameInputHandler, saveFileSelector);
            //game.Start();
            MultiGameManager multiGameManager = new MultiGameManager(renderer, gameLogic, gameSetupInputHandler, gameFieldAnalyzer, fileManager, gameInputHandler, saveFileSelector);
            multiGameManager.Start();
        }
    }
}
