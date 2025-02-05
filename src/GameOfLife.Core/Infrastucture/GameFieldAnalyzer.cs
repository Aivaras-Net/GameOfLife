using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Infrastucture
{
    public class GameFieldAnalyzer : IGameFieldAnalyzer
    {
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
