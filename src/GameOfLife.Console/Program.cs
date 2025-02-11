using GameOfLife.CLI.Infrastructure;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Infrastucture;
using GameOfLife.Core.Infrastructure;

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
            IGameCommandHandler commandHandler = new GameCommandHandler(gameInputHandler, renderer);
            MultiGameManager multiGameManager = new MultiGameManager(renderer, gameLogic, gameSetupInputHandler, gameFieldAnalyzer, fileManager, saveFileSelector,commandHandler);
            multiGameManager.Start();
        }
    }
}
