namespace GameOfLife.Core.Interfaces
{
    public interface IRenderer
    {
        /// <summary>
        /// Renders the game field along with controls and statistics.
        /// </summary>
        /// <param name="field">A two-dimensional boolean array representing the state of cells in the game field.</param>
        /// <param name="iteration">The current iteration count.</param>
        /// <param name="livingCells">The number of living cells.</param>
        /// <param name="offsetX">The left offset for rendering.</param>
        /// <param name="offsetY">The top offset for rendering.</param>
        void Render(bool[,] field,int gameID, int iteration, int livingCells, bool paused, int offsetX, int offsetY);

        /// <summary>
        /// Renders game statistics above the game field.
        /// </summary>
        /// <param name="iteration">The current iteration count.</param>
        /// <param name="livingCells">The number of living cells.</param>
        /// <param name="offsetX">The left offset for rendering the statistics.</param>
        /// <param name="offsetY">The top offset for rendering the statistics.</param>
        //void RenderStatistics(int iteration, int livingCells, int offsetX, int offsetY);

        /// <summary>
        /// Renders a temporary message.
        /// </summary>
        /// <param name="message">The message to display.</param>
        void RenderMessage(string message);

        /// <summary>
        /// Initiates an empty off-screen buffer.
        /// </summary>
        public void BeginFrame();

        /// <summary>
        /// Flushes the off–screen buffer to the console.
        /// </summary>
        public void Flush();

        /// <summary>
        /// Displays a prompt message and retrieves the user input.
        /// </summary>
        /// <param name="message">The prompt message to display.</param>
        /// <returns>The user's input as a string.</returns>
        public string Prompt(string message);


    }
}
