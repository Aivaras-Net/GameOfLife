namespace GameOfLife.Core.Interfaces
{
    public interface IGameFieldAnalyzer
    {
        /// <summary>
        /// Counts the number of living cells in the field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns>Number of living cells</returns>
        int CountLivingCells(bool[,] field);
    }
}
