using System.Text.Json;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Infrastructure
{
    /// <summary>
    /// Manages saving and loading game state to and from files.
    /// </summary>
    public class FileManager : IFileManager
    {
        /// <summary>
        /// Saves the current game state to a file in the specified directory.
        /// </summary>
        /// <param name="game">The game instance to save.</param>
        /// <param name="directoryPath">Directory path to save the game state.</param>
        public void SaveGame(GameInstance game, string directoryPath)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException(Constants.NullOrEmptyDirectoryPathMessage, Constants.DirectoryPathArgumentName);

            Directory.CreateDirectory(directoryPath);
            int saveCount = Directory.GetFiles(directoryPath, Constants.SingleFileSearchPattern).Length;
            string filePath = Path.Combine(directoryPath,
                $"{Constants.SingleSaveFilePrefix}{saveCount + 1}{Constants.SaveFileExtension}");

            var gameStateData = GameStateData.FromGameInstance(game);
            string json = JsonSerializer.Serialize(gameStateData, new JsonSerializerOptions { WriteIndented = Constants.JsonWriteIndented });
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Saves multiple game states to a single file in the specified directory.
        /// </summary>
        /// <param name="games">List of game instances to save.</param>
        /// <param name="directoryPath">Directory path to save the game states.</param>
        public void SaveAllGames(List<GameInstance> games, string directoryPath)
        {
            if (games == null) throw new ArgumentNullException(nameof(games));
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException(Constants.NullOrEmptyDirectoryPathMessage, Constants.DirectoryPathArgumentName);

            Directory.CreateDirectory(directoryPath);
            int saveCount = Directory.GetFiles(directoryPath, Constants.MultipleSaveFileSearchPattern).Length;
            string filePath = Path.Combine(directoryPath, $"{Constants.MultipleSaveFilePrefix}{saveCount + 1}{Constants.SaveFileExtension}");

            var gameStates = games.Select(GameStateData.FromGameInstance).ToArray();
            var combinedState = new CombinedGameState { GameStates = gameStates };

            string json = JsonSerializer.Serialize(combinedState, new JsonSerializerOptions { WriteIndented = Constants.JsonWriteIndented });
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Loads a saved game state from the specified file.
        /// </summary>
        /// <param name="filePath">Path to the saved game file.</param>
        /// <returns>A GameInstance containing the loaded game state.</returns>
        public GameInstance LoadGame(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(Constants.FilePathArgumentName, Constants.NullOrEmptyFilePathMessage);

            string json = File.ReadAllText(filePath);
            var gameStateData = JsonSerializer.Deserialize<GameStateData>(json);
            if (gameStateData?.Field == null)
                throw new Exception(Constants.InvalidGameStateDataMessage);

            return gameStateData.ToGameInstance(1);
        }

        /// <summary>
        /// Loads multiple game states from the specified file.
        /// </summary>
        /// <param name="filePath">Path to the file containing saved multiple game states.</param>
        /// <returns>A list of GameInstance objects containing the loaded game states.</returns>
        public List<GameInstance> LoadMultipleGames(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(Constants.FilePathArgumentName, Constants.NullOrEmptyFilePathMessage);

            string json = File.ReadAllText(filePath);
            var combinedGame = JsonSerializer.Deserialize<CombinedGameState>(json);
            if ((combinedGame?.GameStates?.Length ?? 0) == 0)
                throw new Exception(Constants.InvalidGameStateDataMessage);

            return combinedGame.GameStates
                .Select((state, index) => state.ToGameInstance(index + 1))
                .ToList();
        }
    }
}

