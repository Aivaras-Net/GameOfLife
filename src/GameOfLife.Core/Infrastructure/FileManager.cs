using System.Text.Json;
using GameOfLife.Core.Interfaces;

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
        /// <param name="field">2D boolean array representing the game field.</param>
        /// <param name="iteration">Current iteration count.</param>
        /// <param name="directoryPath">Directory path to save the game state.</param>
        public void SaveGame(bool[,] field, int iteration, string directoryPath)
        {
            if (field == null)
                throw new ArgumentNullException(Constants.FieldArgumentName);
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException(Constants.NullOrEmptyDirectoryPathMessage, Constants.DirectoryPathArgumentName);

            Directory.CreateDirectory(directoryPath);
            int saveCount = Directory.GetFiles(directoryPath, Constants.SingleFileSearchPattern).Length;
            string filePath = Path.Combine(directoryPath,
                $"{Constants.SingleSaveFilePrefix}{saveCount + 1}{Constants.SaveFileExtension}");

            bool[][] jaggedField = ConvertToJaggedArray(field);

            GameState gameState = new GameState { Field = jaggedField, Iteration = iteration };
            string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = Constants.JsonWriteIndented });
            File.WriteAllText(filePath, json);

        }

        /// <summary>
        /// Saves multiple game states to a single file in the specified directory.
        /// </summary>
        /// <param name="fields">Array of 2D boolean arrays representing multiple game fields.</param>
        /// <param name="iterations">Array of iteration counts for each game state.</param>
        /// <param name="directoryPath">Directory path to save the game states.</param>
        public void SaveAllGames(bool[][,] fields, int[] iterations, string directoryPath)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (iterations == null) throw new ArgumentNullException(nameof(iterations));
            if (fields.Length != iterations.Length)
            {
                throw new ArgumentException(Constants.FieldAndIterationMismatchMessage);
            }
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new ArgumentException(Constants.NullOrEmptyDirectoryPathMessage, Constants.DirectoryPathArgumentName);
            }

            Directory.CreateDirectory(directoryPath);
            int saveCount = Directory.GetFiles(directoryPath, Constants.MultipleSaveFileSearchPattern).Length;
            string filePath = Path.Combine(directoryPath, $"{Constants.MultipleSaveFilePrefix}{saveCount + 1}{Constants.SaveFileExtension}");

            List<GameState> gameStates = new List<GameState>();
            for (int index = 0; index < fields.Length; index++)
            {
                bool[][] jaggedField = ConvertToJaggedArray(fields[index]);
                gameStates.Add(new GameState { Field = jaggedField, Iteration = iterations[index] });
            }

            CombinedGameState combinedState = new CombinedGameState { GameStates = gameStates.ToArray() };
            string json = JsonSerializer.Serialize(combinedState, new JsonSerializerOptions { WriteIndented = Constants.JsonWriteIndented });
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
                throw new ArgumentNullException(Constants.FilePathArgumentName, Constants.NullOrEmptyFilePathMessage);

            string json = File.ReadAllText(filePath);
            GameState gameState = JsonSerializer.Deserialize<GameState>(json);
            if ((gameState?.Field?.Length ?? 0) == 0)
            {
                throw new Exception(Constants.InvalidGameStateDataMessage);
            }

            bool[,] field = ConvertTo2DArray(gameState.Field);
            return (field, gameState.Iteration);
        }

        /// <summary>
        /// Loads multiple game states from the specified file.
        /// </summary>
        /// <param name="filePath">Path to the file containing saved multiple game states.</param>
        /// <returns>A tuple containing an array of game fields and an array of iteration counts.</returns>
        public (bool[][,] fields, int[] iterarions) LoadMultipleGames(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(Constants.FilePathArgumentName, Constants.NullOrEmptyFilePathMessage);
            }

            string json = File.ReadAllText(filePath);
            CombinedGameState combinedGame = JsonSerializer.Deserialize<CombinedGameState>(json);
            if ((combinedGame?.GameStates?.Length ?? 0) == 0)
            {
                throw new Exception(Constants.InvalidGameStateDataMessage);
            }

            int gameCount = combinedGame.GameStates.Length;
            bool[][,] fields = new bool[gameCount][,];
            int[] iterations = new int[gameCount];

            for (int index = 0; index < gameCount; index++)
            {
                GameState gameState = combinedGame.GameStates[index];
                if ((gameState?.Field?.Length ?? 0) == 0)
                {
                    throw new Exception(Constants.InvalidGameStateDataMessage);
                }
                fields[index] = ConvertTo2DArray(gameState.Field);
                iterations[index] = gameState.Iteration;
            }

            return (fields, iterations);
        }

        /// <summary>
        /// Converts a 2D boolean array to a jagged boolean array
        /// </summary>
        /// <param name="array">The 2D array to convert</param>
        /// <returns>A jagged array representing the same data</returns>
        private bool[][] ConvertToJaggedArray(bool[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            bool[][] jagged = new bool[rows][];

            for (int i = 0; i < rows; i++)
            {
                jagged[i] = new bool[cols];
                for (int j = 0; j < cols; j++)
                {
                    jagged[i][j] = array[i, j];
                }
            }
            return jagged;
        }

        /// <summary>
        /// Converts a jagged boolean array to a 2D boolean array
        /// </summary>
        /// <param name="jagged">The jagged array to convert</param>
        /// <returns>A 2D array representing the same data</returns>
        private bool[,] ConvertTo2DArray(bool[][] jagged)
        {
            int rows = jagged.Length;
            int cols = jagged[0].Length;
            bool[,] array = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = jagged[i][j];
                }
            }
            return array;
        }
    }
}

