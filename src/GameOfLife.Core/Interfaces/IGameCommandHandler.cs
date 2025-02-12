using System;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Defines methods for processing game commands.
    /// </summary>
    public interface IGameCommandHandler
    {
        /// <summary>
        /// Processes a game command.
        /// Returns false if the Quit command is issued; otherwise, returns true.
        /// </summary>
        /// <param name="numberOfGames">Total number of running games.</param>
        /// <param name="onSaveAll">Action to save all games.</param>
        /// <param name="onSaveSingle">Action to save a specific game (by zero-based index).</param>
        /// <param name="onTogglePauseAll">Action to toggle pause state for all games.</param>
        /// <param name="onTogglePauseSingle">Action to toggle pause state for a specific game (by zero-based index).</param>
        /// <returns>True if processing should continue; otherwise, false.</returns>
        bool ProcessCommand(
            int numberOfGames,
            Action onSaveAll,
            Action<int> onSaveSingle,
            Action onTogglePauseAll,
            Action<int> onTogglePauseSingle);
    }
}
