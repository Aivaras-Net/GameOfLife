using System.Text.Json;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Infrastucture
{
    /// <summary>
    /// Manages saving and loading game state to and from files.
    /// </summary>
    public class FileManager : IFileManager
    {
        /// <summary>
        /// Saves the current game state to a file in the specified directory.
        /// </summary>
        /// <param name="field">2D boolean array representing the game field.</param>
        /// <param name="iteration">Current iteration count.</param>
        /// <param name="directoryPath">Directory path to save the game state.</param>
        public void SaveGame(bool[,] field, int iteration, string directoryPath)
        {
            if (field == null) throw new ArgumentNullException("field");
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Directory path cannot be null or empty.", nameof(directoryPath));

            Directory.CreateDirectory(directoryPath);
            int saveCount = Directory.GetFiles(directoryPath, "SavedGame*.json").Length;
            string filePath = Path.Combine(directoryPath, $"SavedGame{saveCount + 1}.json");

            int rows = field.GetLength(0);
            int cols = field.GetLength(1);
            bool[][] jaggedField = new bool[rows][];
            for (int i = 0; i < rows; i++)
            {
                jaggedField[i] = new bool[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedField[i][j] = field[i,j];
                }
            }

            GameState gameState = new GameState { Field=jaggedField, Iteration= iteration};
            string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

        }

        /// <summary>
        /// Loads a saved game state from the specified file.
        /// </summary>
        /// <param name="filePath">Path to the saved game file.</param>
        /// <returns>A tuple containing the game field and iteration count.</returns>
        public (bool[,] field, int iteration) LoadGame(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("File path cannot be null or empty.", nameof(filePath));
            }

            string json = File.ReadAllText(filePath);
            GameState gameState = JsonSerializer.Deserialize<GameState>(json);
            if(gameState?.Field == null || gameState.Field.Length == 0)
            {
                throw new Exception("Invalid game state data");    
            }

            int rows = gameState.Field.Length;
            int cols = gameState.Field[0].Length;
            bool[,] field = new bool[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0;j < cols; j++)
                {
                    field[i,j] = gameState.Field[i][j];
                }
            }
            return (field,gameState.Iteration);
        }
    }
}

