using GameOfLife.Core.Infrastucture;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Infrastructure
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
        private readonly ISaveFileSelector _saveFileSelector;
        private readonly IGameCommandHandler _gameCommandHandler;

        private bool[][,] _fields;
        private bool[] _paused;
        private int[] _iterations;
        private int _fieldSize;
        private int _numberOfGames;

        public MultiGameManager(IRenderer renderer,
                                IGameLogic gameLogic,
                                IGameSetupInputHandler inputHandler,
                                IGameFieldAnalyzer gameFieldAnalyzer,
                                IFileManager fileManager,
                                ISaveFileSelector saveFileSelector,
                                IGameCommandHandler gameCommandHandler)
        {
            _renderer = renderer;
            _gameLogic = gameLogic;
            _gameSetupInputHandler = inputHandler;
            _gameFieldAnalyzer = gameFieldAnalyzer;
            _gameFileManager = fileManager;
            _saveFileSelector = saveFileSelector;
            _gameCommandHandler = gameCommandHandler;
        }

        /// <summary>
        /// Starts the main game loop.
        /// </summary>
        public void Start()
        {
            if (!SetupGame())
                return;

            int headerHeight = Constants.Headerheight;
            while (true)
            {
                bool continueGame = _gameCommandHandler.ProcessCommand(
                    _numberOfGames,
                    onSaveAll: () => _gameFileManager.SaveAllGames(_fields, _iterations, Constants.DefaultSaveFolder),
                    onSaveSingle: (index) => _gameFileManager.SaveGame(_fields[index], _iterations[index], Constants.DefaultSaveFolder),
                    onTogglePauseAll: () =>
                    {
                        for (int i = 0; i < _numberOfGames; i++)
                        {
                            _paused[i] = !_paused[i];
                        }
                    },
                    onTogglePauseSingle: (index) =>
                    {
                        _paused[index] = !_paused[index];
                    });

                if (!continueGame)
                    break;

                RenderFrame(headerHeight);
                UpdateGames();
                Thread.Sleep(Constants.DefaultSleepTime);
            }
        }

        /// <summary>
        /// Performs initial game setup based on the chosen start mode.
        /// </summary>
        /// <returns>True if setup was successful; otherwise, false.</returns>
        private bool SetupGame() //Would be better in their own class but there are enought classes already for this small project
        {
            GameStartMode startMode = _gameSetupInputHandler.GetGameStartMode();
            if (startMode == GameStartMode.Load)
            {
                return SetupFromLoad();
            }
            else if (startMode == GameStartMode.ParralelShowcase)
            {
                _renderer.RenderMessage(Constants.ParallelShowcaseNotImplementedMessage);
                _renderer.Flush();
                Thread.Sleep(Constants.DefaultSleepTime);
                return false;
            }
            else
            {
                return SetupNewGame();
            }
        }

        private bool SetupFromLoad()
        {
            string filePath = _saveFileSelector.SelectSavedFilePath();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _renderer.RenderMessage(Constants.NoValidFileSelectedMessage);
                _renderer.Flush();
                return false;
            }

            if (Path.GetFileName(filePath).StartsWith(Constants.MultipleSaveFilePrefix))
            {
                var (loadedFields, loadedIterations) = _gameFileManager.LoadMultipleGames(filePath);
                _numberOfGames = loadedFields.Length;
                _fieldSize = loadedFields[0].GetLength(0);
                _fields = loadedFields;
                _iterations = loadedIterations;
            }
            else
            {
                var (loadedField, loadedIteration) = _gameFileManager.LoadGame(filePath);
                _numberOfGames = 1;
                _fieldSize = loadedField.GetLength(0);
                _fields = new bool[1][,] { loadedField };
                _iterations = new int[1] { loadedIteration };
            }
            _paused = new bool[_numberOfGames];
            _renderer.RenderMessage(Constants.GameLoadedSuccessfullyMessage);
            _renderer.Flush();
            Thread.Sleep(Constants.DefaultSleepTime);
            return true;
        }

        private bool SetupNewGame()
        {
            _numberOfGames = _gameSetupInputHandler.GetNumberOfGames();
            _fieldSize = _gameSetupInputHandler.GetFieldSize();
            _fields = new bool[_numberOfGames][,];
            _iterations = new int[_numberOfGames];
            _paused = new bool[_numberOfGames];

            for (int i = 0; i < _numberOfGames; i++)
            {
                _fields[i] = InitializeField(_fieldSize);
                _iterations[i] = 0;
                _paused[i] = false;
            }
            return true;
        }

        /// <summary>
        /// Initializes a game field with a random configuration.
        /// </summary>
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

        /// <summary>
        /// Renders the current frame of all games.
        /// </summary>
        private void RenderFrame(int headerHeight)
        {
            _renderer.BeginFrame();
            int boardWidth = _fieldSize + 10; // Temporary magic number for formatting.
            int boardHeight = _fieldSize + 5;
            int maxColumns = Math.Max(1, Console.WindowWidth / boardWidth);
            int columns = Math.Min(_numberOfGames, maxColumns);

            for (int i = 0; i < _numberOfGames; i++)
            {
                int colIndex = i % columns;
                int rowIndex = i / columns;
                int offsetX = colIndex * boardWidth;
                int offsetY = headerHeight + rowIndex * boardHeight;

                int livingCells = _gameFieldAnalyzer.CountLivingCells(_fields[i]);
                _renderer.Render(_fields[i], i + 1, _iterations[i], livingCells, _paused[i], offsetX, offsetY);
            }
            _renderer.Flush();
        }

        /// <summary>
        /// Updates the state of each game that is not paused.
        /// </summary>
        private void UpdateGames()
        {
            for (int i = 0; i < _numberOfGames; i++)
            {
                if (!_paused[i])
                {
                    _fields[i] = _gameLogic.ComputeNextState(_fields[i]);
                    _iterations[i]++;
                }
            }
        }
    }
}
