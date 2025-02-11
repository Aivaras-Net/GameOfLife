﻿using GameOfLife.Core.Interfaces;

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

        private bool[][,] _fields;
        private bool[] _paused;
        private int[] _iterations;

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
        }

        /// <summary>
        /// Starts the loop updating and rendering all games.
        /// </summary>
        public void Start()
        {
            GameStartMode startMode = _gameSetupInputHandler.GetGameStartMode();
            int numberOfGames;
            int fieldSize;
            
            if (startMode == GameStartMode.Load)
            {
                string filePath = _saveFileSelector.SelectSavedFilePath();
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    if (Path.GetFileName(filePath).StartsWith(Constants.MultipleSaveFilePrefix))
                    {
                        var (loadedFields, loadedIteration) = _gameFileManager.LoadMultipleGames(filePath);
                        numberOfGames = loadedFields.Length;
                        fieldSize = loadedFields[0].GetLength(0);
                        _fields = loadedFields;
                        _iterations = loadedIteration;
                        _paused = new bool[numberOfGames];
                        _renderer.RenderMessage("Loaded multiple games successfully.");
                    }
                    else
                    {
                        var (loadedField, loadedIteration) = _gameFileManager.LoadGame(filePath);
                        numberOfGames = 1;
                        fieldSize = loadedField.GetLength(0);
                        _fields = new bool[1][,] { loadedField };
                        _iterations = new int[1] { loadedIteration };
                        _paused = new bool[1];
                        _renderer.RenderMessage("Loaded game successfully");
                    }
                    _renderer.Flush();
                    Thread.Sleep(Constants.DefaultSleepTime);
                }
                else
                {
                    Console.WriteLine("AAA");
                    return;
                }
            }
            else if (startMode == GameStartMode.ParralelShowcase)
            {
                _renderer.RenderMessage("Parallel Showcase is not implemented yet.");
                _renderer.Flush();
                Thread.Sleep(Constants.DefaultSleepTime);
                return;
            }
            else
            {
                numberOfGames=_gameSetupInputHandler.GetNumberOfGames();
                fieldSize=_gameSetupInputHandler.GetFieldSize();
                _fields = new bool[numberOfGames][,];
                _iterations = new int[numberOfGames];
                _paused = new bool[numberOfGames];
                for (int i = 0; i < numberOfGames; i++)
                {
                    _fields[i] = InitializeField(fieldSize);
                    _iterations[i] = 0;
                    _paused[i] = false;
                }
            }
            int headerHeight = Constants.Headerheight;

            while (true)
            {
                GameCommand command = _gameInputHandler.GetCommand();
                if (command == GameCommand.Quit)
                {
                    _renderer.RenderMessage(Constants.ExitingGameMessage);
                    _renderer.Flush();
                    break;
                }

                if (command == GameCommand.Save)
                {
                    if (numberOfGames > 1)
                    {
                        string input = _renderer.Prompt("Enter 0 to save all games in one file or a game number (1-" + numberOfGames + ") to save a specific game:");
                        if (int.TryParse(input, out int selection))
                        {
                            if (selection == 0)
                            {
                                _gameFileManager.SaveAllGames(_fields, _iterations, Constants.DefaultSaveFolder);
                                _renderer.RenderMessage("All games saved successfully.");
                            }
                            else if (selection >= 1 && selection <= numberOfGames)
                            {
                                _gameFileManager.SaveGame(_fields[selection - 1], _iterations[selection - 1], Constants.DefaultSaveFolder);
                                _renderer.RenderMessage($"Game {selection} saved successfully");
                            }
                            else
                            {
                                _renderer.RenderMessage( "Invalid selection for saving.");
                            }
                        }
                        else
                        {
                            _renderer.RenderMessage("Invalid input, Please enter a number");
                        }
                    }
                    else
                    {
                        _gameFileManager.SaveGame(_fields[0], _iterations[0], Constants.DefaultSaveFolder);
                        _renderer.RenderMessage(Constants.GameSavedMessage);
                    }
                }

                if (command == GameCommand.Stop)
                {
                    if (numberOfGames > 1)
                    {
                        string input =_renderer.Prompt("Enter 0 to toggle pause state for all games or a game number (1-" + numberOfGames + ") for a specific game:");
                        if( int.TryParse(input, out int selection))
                            if (selection == 0)
                            {
                                for (int i = 0; i < numberOfGames; i++)
                                {
                                    _paused[i] = !_paused[i];
                                }
                                _renderer.RenderMessage("Toggled pause state for all games.");
                            }
                        else if (selection >= 1 && selection <= numberOfGames)
                            {
                                _paused[selection -1] = !_paused[selection -1];
                                _renderer.RenderMessage($"Game {selection} pause state changed");
                            }
                        else
                            {
                                _renderer.RenderMessage("Invalid selection for toffling pause state");
                            }
                    }
                    else
                    {
                        _paused[0] = !_paused[0];
                        _renderer.RenderMessage(_paused[0] ? "Game paused." : "Game resumed");
                    }
                }

                // Begin new frame by initializing the off–screen buffer.
                _renderer.BeginFrame();

                int boardWidth = (fieldSize) + 10; //magic numbers until the formatting of the field is finalised
                int boardHeight = fieldSize + 5;
                int maxColumns = Math.Max(1, Console.WindowWidth / boardWidth);
                int columns = Math.Min(numberOfGames, maxColumns);

                for (int i = 0; i < numberOfGames; i++)
                {
                    int colIndex = i % columns;
                    int rowIndex = i / columns;
                    int offsetX = colIndex * boardWidth;
                    int offsetY = headerHeight + rowIndex * boardHeight;

                    int livingCells = _gameFieldAnalyzer.CountLivingCells(_fields[i]);
                    _renderer.Render(_fields[i],i+1, _iterations[i], livingCells, _paused[i], offsetX, offsetY);

                    if (!_paused[i])
                    {
                        _fields[i] = _gameLogic.ComputeNextState(_fields[i]);
                        _iterations[i]++;
                    }
                }

                // Flush the complete frame to the console.
                _renderer.Flush();
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
