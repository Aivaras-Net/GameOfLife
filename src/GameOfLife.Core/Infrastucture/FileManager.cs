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
            if (field == null)
                throw new ArgumentNullException(Constants.FieldArgumentName);
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException(Constants.NullOrEmptyDirectoryPathMessage, Constants.DirectoryPathArgumentName);

            Directory.CreateDirectory(directoryPath);
            int saveCount = Directory.GetFiles(directoryPath, Constants.SingleFileSearchPattern).Length;
            string filePath = Path.Combine(directoryPath,
                $"{Constants.SingleSaveFilePrefix}{saveCount + 1}{Constants.SaveFileExtension}");

            int rows = field.GetLength(0);
            int cols = field.GetLength(1);
            bool[][] jaggedField = new bool[rows][];
            for (int i = 0; i < rows; i++)
            {
                jaggedField[i] = new bool[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedField[i][j] = field[i, j];
                }
            }

            GameState gameState = new GameState { Field = jaggedField, Iteration = iteration };
            string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = Constants.JsonWriteIndented });
            File.WriteAllText(filePath, json);

        }


        public void SaveAllGames(bool[][,] fields, int[] iterations, string directoryPath)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (iterations == null) throw new ArgumentNullException(nameof(iterations));
            if (fields.Length != iterations.Length)
            {
                throw new ArgumentException("Mismatch between fields and iterations length");
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
                bool[,] field = fields[index];
                int iteration = iterations[index];

                int rows = field.GetLength(0);
                int cols = field.GetLength(1);
                bool[][] jaggedField = new bool[rows][];
                for (int i = 0; i < rows; i++)
                {
                    jaggedField[i] = new bool[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        jaggedField[i][j] = field[i, j];
                    }
                }
                gameStates.Add(new GameState { Field = jaggedField, Iteration = iteration });
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
            if (gameState?.Field == null || gameState.Field.Length == 0)
            {
                throw new Exception(Constants.InvalidGameStateDataMessage);
            }

            int rows = gameState.Field.Length;
            int cols = gameState.Field[0].Length;
            bool[,] field = new bool[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    field[i, j] = gameState.Field[i][j];
                }
            }
            return (field, gameState.Iteration);
        }
    }
}

