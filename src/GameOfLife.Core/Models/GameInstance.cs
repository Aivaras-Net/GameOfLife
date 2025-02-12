using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// Represents a single instance of the Game of Life simulation.
    /// </summary>
    public class GameInstance
    {
        public bool[,] Field { get; private set; }

        public int Iteration { get; private set; }

        public bool IsPaused { get; set; }

        public int LivingCells { get; private set; }

        public int Id { get; }

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