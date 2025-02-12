using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Infrastructure
{
    /// <summary>
    /// Manages initiation and execution of single or multiple simultaneous games.
    /// </summary>
    public class MultiGameManager
    {
        private readonly IRenderer _renderer;
        private readonly IGameLogic _gameLogic;
        private readonly IGameSetupInputHandler _gameSetupInputHandler;
        private readonly IFileManager _gameFileManager;
        private readonly ISaveFileSelector _saveFileSelector;
        private readonly IGameCommandHandler _gameCommandHandler;

        private List<GameInstance> _games;
        private int _fieldSize;
        private int _displayedGameIndex = 0;

        public MultiGameManager(IRenderer renderer,
                                IGameLogic gameLogic,
                                IGameSetupInputHandler inputHandler,
                                IFileManager fileManager,
                                ISaveFileSelector saveFileSelector,
                                IGameCommandHandler gameCommandHandler)
        {
            _renderer = renderer;
            _gameLogic = gameLogic;
            _gameSetupInputHandler = inputHandler;
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
                    _games.Count,
                    onSaveAll: () => _gameFileManager.SaveAllGames(_games, Constants.DefaultSaveFolder),
                    onSaveSingle: (index) => _gameFileManager.SaveGame(_games[index], Constants.DefaultSaveFolder),
                    onTogglePauseAll: () =>
                    {
                        foreach (var game in _games)
                        {
                            game.IsPaused = !game.IsPaused;
                        }
                    },
                    onTogglePauseSingle: (index) =>
                    {
                        _games[index].IsPaused = !_games[index].IsPaused;
                    },
                    onViewGame: (index) =>
                    {
                        _displayedGameIndex = index;
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
        private bool SetupGame()
        {
            GameStartMode startMode = _gameSetupInputHandler.GetGameStartMode();
            if (startMode == GameStartMode.Load)
            {
                return SetupFromLoad();
            }
            else if (startMode == GameStartMode.ParralelShowcase)
            {
                return SetupParallelShowcase();
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

            try
            {
                if (Path.GetFileName(filePath).StartsWith(Constants.MultipleSaveFilePrefix))
                {
                    _games = _gameFileManager.LoadMultipleGames(filePath).ToList();
                }
                else
                {
                    _games = new List<GameInstance> { _gameFileManager.LoadGame(filePath) };
                }
                _fieldSize = _games[0].Field.GetLength(0);

                _renderer.RenderMessage(Constants.GameLoadedSuccessfullyMessage);
                _renderer.Flush();
                Thread.Sleep(Constants.DefaultSleepTime);
                return true;
            }
            catch (Exception ex)
            {
                _renderer.RenderMessage(string.Format(Constants.LoadGameFailedMessageFormat, ex.Message));
                _renderer.Flush();
                Thread.Sleep(Constants.DefaultSleepTime);
                return false;
            }
        }

        private bool SetupNewGame()
        {
            int numberOfGames = _gameSetupInputHandler.GetNumberOfGames();
            _fieldSize = _gameSetupInputHandler.GetFieldSize();
            _games = new List<GameInstance>();

            for (int i = 0; i < numberOfGames; i++)
            {
                var field = InitializeField(_fieldSize);
                _games.Add(new GameInstance(i + 1, field));
            }
            return true;
        }

        private bool SetupParallelShowcase()
        {
            _fieldSize = Constants.ParallelShowcaseFieldSize;
            _games = new List<GameInstance>();

            Parallel.For(0, Constants.ParallelShowcaseGameCount, i =>
            {
                var field = InitializeField(_fieldSize);
                var game = new GameInstance(i + 1, field);
                lock (_games)
                {
                    _games.Add(game);
                }
            });

            // Sort games by ID to ensure the list index matches the game ID
            _games = _games.OrderBy(g => g.Id).ToList();

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
            _renderer.BeginFrame(_games.Count == Constants.ParallelShowcaseGameCount);

            int activeGames = _games.Count(g => !g.IsPaused);
            int totalLivingCells = _games.Sum(g => g.LivingCells);

            _renderer.RenderGlobalStats(activeGames, totalLivingCells);

            if (_games.Count == Constants.ParallelShowcaseGameCount)
            {
                var game = _games[_displayedGameIndex];
                _renderer.Render(game.Field, game.Id, game.Iteration,
                               game.LivingCells, game.IsPaused, 0, headerHeight);
            }
            else
            {
                int boardWidth = Math.Max(_fieldSize + 2, Constants.MinimumStatsWidth); //+2 for borders
                int boardHeight = _fieldSize + 4; // 2 for borders + 1 for stats + extra padding
                int maxColumns = Math.Max(1, Console.WindowWidth / boardWidth);
                int columns = Math.Min(_games.Count, maxColumns);

                for (int i = 0; i < _games.Count; i++)
                {
                    int colIndex = i % columns;
                    int rowIndex = i / columns;
                    int offsetX = colIndex * (boardWidth + 1);
                    int offsetY = headerHeight + rowIndex * boardHeight;

                    var game = _games[i];
                    _renderer.Render(game.Field, game.Id, game.Iteration,
                                   game.LivingCells, game.IsPaused, offsetX, offsetY);
                }
            }
            _renderer.Flush();
        }

        /// <summary>
        /// Updates the state of each game that is not paused.
        /// </summary>
        private void UpdateGames()
        {
            Parallel.ForEach(_games, game => game.UpdateState(_gameLogic));
        }
    }
}
