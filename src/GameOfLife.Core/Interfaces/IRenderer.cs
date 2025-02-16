﻿namespace GameOfLife.Core.Interfaces
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
        void Render(bool[,] field, int gameId, int iteration, int livingCells, bool paused = false, int offsetX = 0, int offsetY = 0);

        /// <summary>
        /// Renders a temporary message.
        /// </summary>
        /// <param name="message">The message to display.</param>
        void RenderMessage(string message);

        /// <summary>
        /// Initiates an empty off-screen buffer.
        /// </summary>
        void BeginFrame(bool isParallelShowcase = false);

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

        /// <summary>
        /// Renders the global statistics for all games.
        /// </summary>
        /// <param name="activeGames">Number of active games</param>
        /// <param name="totalLivingCells">Number of total living cells</param>
        void RenderGlobalStats(int activeGames, int totalLivingCells);
    }
}
