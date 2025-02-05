namespace GameOfLife.Core.Interfaces
{
    public interface IRenderer
    {
        /// <summary>
        /// Renders the current state of the game.
        /// </summary>
        /// <param name="field">A two dimentional boolean array representing the state of cells in a game field.</param>
        void Render(bool[,] field);

        void RenderStatistics(int iteration, int livingCells, int fieldHeight);
    }
}
