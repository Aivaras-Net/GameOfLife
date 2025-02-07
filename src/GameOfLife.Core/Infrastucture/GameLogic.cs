using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Infrastucture
{
    /// <summary>
    /// Implements the rules of 'Game of Life' to compute following states.
    /// </summary>
    public class GameLogic : IGameLogic
    {
        /// <summary>
        /// Computes the next state of the game field by evaluating every cells neighbor count.
        /// </summary>
        /// <param name="currentField">Two dimentional boolean array representing the current game field.</param>
        /// <returns>Two dimentional boolean array representing the next game field.</returns>
        public bool[,] ComputeNextState(bool[,] currentField)
        {
            int rows = currentField.GetLength(0);
            int cols = currentField.GetLength(1);

            bool[,] result = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int aliveNeighbors = CountAliveNeighbors(currentField, i, j);
                    // Apply Conway's rules.
                    if (currentField[i, j])
                    {
                        result[i, j] = aliveNeighbors == 2 || aliveNeighbors == 3;
                    }
                    else
                    {
                        result[i, j] = aliveNeighbors == 3;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Counts the number of alive neighboring cells for a given cell.
        /// </summary>
        /// <param name="field">Two dimentional boolean array representing the game field.</param>
        /// <param name="row">The row index of the cell.</param>
        /// <param name="col">The column index of the cell.</param>
        /// <returns>The count of alive neighbors.</returns>
        static int CountAliveNeighbors(bool[,] field, int row, int col)
        {
            int count = 0;
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i == row && j == col) continue;
                    if (i >= 0 && i < rows && j >= 0 && j < cols && field[i, j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
