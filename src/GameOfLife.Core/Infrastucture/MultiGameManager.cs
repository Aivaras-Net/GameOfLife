using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Infrastucture
{
    /// <summary>
    /// Manages initiation and execution of multiple simultaneous games.
    /// </summary>
    public class MultiGameManager
    {
        private readonly IRenderer _renderer;
        private readonly IGameLogic _gameLogic;
        private readonly IGameSetupInputHandler _gameSetupInputHandler;
        private readonly IGameFieldAnalyzer _gameFieldAnalyzer;
        private readonly IFileManager _gameFileManager;
        private readonly IGameInputHandler _gameInputHandler;
        private readonly ISaveFileSelector _saveFileSelector;

        private const int NumberOfGames = 8;
        private readonly bool[][,] _fields;
        private readonly int[] _iterations;

        public MultiGameManager(IRenderer renderer,
                                IGameLogic gameLogic,
                                IGameSetupInputHandler inputHandler,
                                IGameFieldAnalyzer gameFieldAnalyzer,
                                IFileManager fileManager,
                                IGameInputHandler gameInputHandler,
                                ISaveFileSelector saveFileSelector)
        {
            _renderer = renderer;
            _gameLogic = gameLogic;
            _gameSetupInputHandler = inputHandler;
            _gameFieldAnalyzer = gameFieldAnalyzer;
            _gameFileManager = fileManager;
            _gameInputHandler = gameInputHandler;
            _saveFileSelector = saveFileSelector;

            _fields = new bool[NumberOfGames][,];
            _iterations = new int[NumberOfGames];
        }

        /// <summary>
        /// Starts the loop updating and rendering all games.
        /// </summary>
        public void Start()
        {
            int fieldSize = _gameSetupInputHandler.GetFieldSize();

            for (int i = 0; i < NumberOfGames; i++)
            {
                _fields[i] = InitializeField(fieldSize);
                _iterations[i] = 0;
            }

            bool stopped = false;

            while (!stopped)
            {
                GameCommand command = _gameInputHandler.GetCommand();

                if (command == GameCommand.Quit)
                {
                    _renderer.RenderMessage(Constants.ExitingGameMessage);
                    break;
                }

                if (command == GameCommand.Save)
                {
                    // Optionally, save each game separately.
                    for (int i = 0; i < NumberOfGames; i++)
                    {
                        string saveFolder = $"{Constants.DefaultSaveFolder}/Game{i + 1}";
                        _gameFileManager.SaveGame(_fields[i], _iterations[i], saveFolder);
                    }
                    _renderer.RenderMessage(Constants.GameSavedMessage);
                }

                //Console.Clear();

                int columns = 4;
                int rows = 2;
                int boardWidth = (fieldSize * 2) + 20;
                int boardHeight = fieldSize + 3;

                for (int i = 0; i < NumberOfGames; i++)
                {
                    int colIndex = i % columns;
                    int rowIndex = i / columns;
                    int offsetX = colIndex * boardWidth;
                    int offsetY = rowIndex * boardHeight;

                    int livingCells = _gameFieldAnalyzer.CountLivingCells(_fields[i]);
                    _renderer.Render(_fields[i], _iterations[i], livingCells, offsetX, offsetY);
                    _fields[i] = _gameLogic.ComputeNextState(_fields[i]);
                    _iterations[i]++;
                }

                Thread.Sleep(Constants.DefaultSleepTime);
            }
        }

        /// <summary>
        /// Initializes a game field with a random configuration.
        /// </summary>
        /// <param name="fieldSize">The size of the square game field.</param>
        /// <returns>A two-dimensional boolean array representing the game field.</returns>
        private static bool[,] InitializeField(int fieldSize)
        {
            bool[,] field = new bool[fieldSize, fieldSize];
            Random random = new Random();

            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    field[i, j] = random.Next(2) == 1;
                }
            }
            return field;
        }
    }
}
