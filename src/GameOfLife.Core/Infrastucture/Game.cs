﻿using GameOfLife.Core.Interfaces;


namespace GameOfLife.Core.Infrastucture
{
    /// <summary>
    /// Manages Initiation and execution of the game.
    /// </summary>
    public class Game
    {
        private readonly IRenderer _renderer;
        private readonly IGameLogic _gameLogic;
        private readonly IInputHandler _inputHandler;
        private readonly IGameFieldAnalyzer _gameFieldAnalyzer;
        private bool[,] field;

        public Game(IRenderer renderer, IGameLogic gameLogic, IInputHandler inputHandler, IGameFieldAnalyzer gameFieldAnalyzer)
        {
            _renderer = renderer;
            _gameLogic = gameLogic;
            _inputHandler = inputHandler;
            _gameFieldAnalyzer = gameFieldAnalyzer;
        }

        /// <summary>
        /// Starts the game loop.
        /// </summary>
        public void Start()
        {
            int fieldSize = _inputHandler.GetFieldSize();
            field = InitializeField(fieldSize);
            int iteration = 0;

            // Continuous display and update loop
            while (true)
            {
                _renderer.Render(field);
                int livingCells = _gameFieldAnalyzer.CountLivingCells(field);
                _renderer.RenderStatistics(iteration, livingCells, fieldSize);
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
    }
}
