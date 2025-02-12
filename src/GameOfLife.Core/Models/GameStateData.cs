namespace GameOfLife.Core.Models
{
    /// <summary>
    /// Data transfer object for game state serialization.
    /// </summary>
    public class GameStateData
    {
        /// <summary>
        /// Gets or sets the game field as a jagged array of integers.
        /// </summary>
        public int[][] Field { get; set; }

        /// <summary>
        /// Gets or sets the current iteration count.
        /// </summary>
        public int Iteration { get; set; }

        /// <summary>
        /// Creates a GameStateData instance from a GameInstance.
        /// </summary>
        /// <param name="game">The game instance to convert.</param>
        /// <returns>A new GameStateData object containing the game state.</returns>
        public static GameStateData FromGameInstance(GameInstance game)
        {
            return new GameStateData
            {
                Field = ConvertToJaggedArray(game.Field),
                Iteration = game.Iteration
            };
        }

        /// <summary>
        /// Creates a new GameInstance from this GameStateData.
        /// </summary>
        /// <param name="id">The ID to assign to the new game instance.</param>
        /// <returns>A new GameInstance initialized with this state.</returns>
        public GameInstance ToGameInstance(int id)
        {
            var field = ConvertTo2DArray(Field);
            return new GameInstance(id, field, Iteration);
        }

        /// <summary>
        /// Converts a 2D boolean array to a jagged array of integers.
        /// </summary>
        /// <param name="field">The boolean field to convert.</param>
        /// <returns>A jagged array where 1 represents true and 0 represents false.</returns>
        private static int[][] ConvertToJaggedArray(bool[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);
            int[][] result = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = field[i, j] ? 1 : 0;
                }
            }
            return result;
        }

        /// <summary>
        /// Converts a jagged array of integers to a 2D boolean array.
        /// </summary>
        /// <param name="field">The integer field to convert where 1 represents true and 0 represents false.</param>
        /// <returns>A 2D boolean array representation of the field.</returns>
        private static bool[,] ConvertTo2DArray(int[][] field)
        {
            int rows = field.Length;
            int cols = field[0].Length;
            bool[,] result = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = field[i][j] == 1;
            return result;
        }
    }
}