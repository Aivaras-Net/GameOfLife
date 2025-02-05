using GameOfLife.Interfaces;

namespace GameOfLife.Infrastructure
{
    internal class Game
    {
        private readonly IRenderer _renderer;
        private readonly IGameLogic _gameLogic;
        private readonly IInputHandler _inputHandler;
        private bool[,] field;

        public Game(IRenderer renderer, IGameLogic gameLogic, IInputHandler inputHandler)
        {
            _renderer = renderer;
            _gameLogic = gameLogic;
            _inputHandler = inputHandler;
        }

        public void Start()
        {
            int fieldSize = _inputHandler.GetFieldSize();
            field = InitializeField(fieldSize);

            while (true)
            {
                _renderer.Render(field);
                field = _gameLogic.ComputeNextState(field);
                Thread.Sleep(Constants.DefaultSleepTime);
            }
        }

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
