using GameOfLife.Core.Interfaces;


namespace GameOfLife.Core.Infrastucture
{
    /// <summary>
    /// Manages Initiation and execution of the game.
    /// </summary>
    public class GameManager
    {
        private readonly IRenderer _renderer;
        private readonly IGameLogic _gameLogic;
        private readonly IGameSetupInputHandler _gameSetupInputHandler;
        private readonly IGameFieldAnalyzer _gameFieldAnalyzer;
        private readonly IFileManager _gameFileManager;
        private readonly IGameInputHandler _gameInputHandler;
        private readonly ISaveFileSelector _saveFileSelector;
        private bool[,] field;
        private int iteration;

        public GameManager(IRenderer renderer,
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
        }

        /// <summary>
        /// Starts the game loop.
        /// </summary>
        public void Start()
        {
            iteration = 0;
            GameStartMode startMode = _gameSetupInputHandler.GetGameStartMode();
            if (startMode == GameStartMode.Load)
            {
                string savedGameFile = _saveFileSelector.SelectSavedFilePath();
                    try
                    {
                        var (loadedField, loadedIteration) = _gameFileManager.LoadGame(savedGameFile);
                    field = loadedField;
                    iteration = loadedIteration;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to lad game : {ex.Message} ");
                        return;
                    }
                
            }
            else
            {
                int fieldSize = _gameSetupInputHandler.GetFieldSize();
                field = InitializeField(fieldSize);
            }


            bool stoped = false;

            // Continuous display and update loop
            while (!stoped)
            {
                GameCommand command = _gameInputHandler.GetCommand();
                stoped = ProcessCommand(command);

                if (stoped)
                {
                    break;
                }

                int livingCells = _gameFieldAnalyzer.CountLivingCells(field);
                _renderer.Render(field,iteration,livingCells,0,0); //Offsets are not supposed to be hardcoded, they are made in preparation for future features
                field = _gameLogic.ComputeNextState(field);
                iteration++;
                Thread.Sleep(Constants.DefaultSleepTime);
            }
        }

        /// <summary>
        /// Initializes the game with a random layout of dead and alive cells.
        /// </summary>
        /// <param name="fieldSize">The size of the square game field.</param>
        /// <returns>Two dimentional boolean array representing the game field.</returns>
        private static bool[,] InitializeField(int fieldSize)
        {
            bool[,] field = new bool[fieldSize, fieldSize];
            Random random = new Random(); //will be changed to support specific seeds

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
        /// Processes in-game commands.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>True if the game should stop; otherwise, false.</returns>
        private bool ProcessCommand(GameCommand command)
        {
            switch (command)
            {
                case GameCommand.Save:
                    _gameFileManager.SaveGame(field, iteration, "Saves");
                    _renderer.RenderMessage("Game saved successfully.");
                    break;
                case GameCommand.Quit:
                    _renderer.RenderMessage("Exiting game...");
                    return true;
                default:
                    break;
            }
            return false;
        }
    }
}
