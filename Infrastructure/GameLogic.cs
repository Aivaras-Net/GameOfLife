using GameOfLife.Interfaces;

namespace GameOfLife.Infrastructure
{
    internal class GameLogic : IGameLogic
    {
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
