using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    public interface IFileManager
    {
        /// <summary>
        /// Saves a single game instance to a file.
        /// </summary>
        /// <param name="game">The game instance to save.</param>
        /// <param name="directoryPath">The directory path where the game should be saved.</param>
        void SaveGame(GameInstance game, string directoryPath);

        /// <summary>
        /// Saves multiple game instances to a single file.
        /// </summary>
        /// <param name="games">The list of game instances to save.</param>
        /// <param name="directoryPath">The directory path where the games should be saved.</param>
        void SaveAllGames(List<GameInstance> games, string directoryPath);

        /// <summary>
        /// Loads a single game instance from a file.
        /// </summary>
        /// <param name="filePath">The path to the file containing the saved game.</param>
        /// <returns>A new game instance containing the loaded state.</returns>
        GameInstance LoadGame(string filePath);

        /// <summary>
        /// Loads multiple game instances from a file.
        /// </summary>
        /// <param name="filePath">The path to the file containing the saved games.</param>
        /// <returns>A list of game instances containing the loaded states.</returns>
        List<GameInstance> LoadMultipleGames(string filePath);
    }
}
