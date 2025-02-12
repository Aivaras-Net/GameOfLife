using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Infrastructure
{
    /// <summary>
    /// Provies functionality to analyze the game field.
    /// </summary>
    public class GameFieldAnalyzer : IGameFieldAnalyzer
    {
        /// <summary>
        /// Counts the number of living cells in the field.
        /// </summary>
        /// <param name="field">2D boolean array representing the game field.</param>
        /// <returns>The count of living cells.</returns>
        public int CountLivingCells(bool[,] field)
        {
            int count = 0;
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (field[i, j]) count++;
                }
            }
            return count;
        }
    }
}
