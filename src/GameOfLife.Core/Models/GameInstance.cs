using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// Represents a single instance of the Game of Life simulation.
    /// </summary>
    public class GameInstance
    {
        /// <summary>
        /// Gets the current state of the game field.
        /// </summary>
        public bool[,] Field { get; private set; }

        /// <summary>
        /// Gets the current iteration count.
        /// </summary>
        public int Iteration { get; private set; }

        /// <summary>
        /// Gets or sets whether the game is paused.
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// Gets the current count of living cells.
        /// </summary>
        public int LivingCells { get; private set; }

        /// <summary>
        /// Gets the unique identifier for this game instance.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of the GameInstance class.
        /// </summary>
        /// <param name="id">The unique identifier for this game.</param>
        /// <param name="initialField">The initial state of the game field.</param>
        /// <param name="iteration">The initial iteration count.</param>
        public GameInstance(int id, bool[,] initialField, int iteration = 0)
        {
            Id = id;
            Field = initialField;
            Iteration = iteration;
            IsPaused = false;
            UpdateLivingCells();
        }

        /// <summary>
        /// Updates the game state using the provided game logic.
        /// </summary>
        /// <param name="gameLogic">The game logic to use for the update.</param>
        public void UpdateState(IGameLogic gameLogic)
        {
            if (!IsPaused)
            {
                Field = gameLogic.ComputeNextState(Field);
                Iteration++;
                UpdateLivingCells();
            }
        }

        /// <summary>
        /// Updates the count of living cells in the current field.
        /// </summary>
        private void UpdateLivingCells()
        {
            LivingCells = CountLivingCells(Field);
        }

        /// <summary>
        /// Counts the number of living cells in the given field.
        /// </summary>
        /// <param name="field">The field to count living cells in.</param>
        /// <returns>The number of living cells.</returns>
        private static int CountLivingCells(bool[,] field)
        {
            int count = 0;
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (field[i, j]) count++;
            return count;
        }
    }
}