namespace GameOfLife.Core.Interfaces
{
    public interface IFileManager
    {
        /// <summary>
        /// Saves the game state to a file.
        /// </summary>
        /// <param name="field">A 2D boolean array representing the game field.</param>
        /// <param name="iteration">The current iteration count.</param>
        /// <param name="directoryPath">The path to the directory where the game state should be saved.</param>
        void SaveGame(bool[,] field, int iteration, string directoryPath);

        /// <summary>
        /// Loads the game state from a file.
        /// </summary>
        /// <param name="filePath">The file path from which to load the game state.</param>
        /// <returns> A tuple containing:The 2D boolean array representing the game field;The iteration count.</returns>
        (bool[,] field, int iteration) LoadGame(string filePath);

        /// <summary>
        /// Saves multiple game states to a single file.
        /// </summary>
        /// <param name="fields">An array of 2D boolean arrays representing the game fields of multiple games.</param>
        /// <param name="iterations">An array of iteration counts corresponding to each game state.</param>
        /// <param name="directoryPath">The directory path where the game states should be saved.</param>
        public void SaveAllGames(bool[][,] fields, int[] iterations, string directoryPath);

        /// <summary>
        /// Loads multiple game states from a file.
        /// </summary>
        /// <param name="filePath">The file path from which to load the game states</param>
        /// <returns>A tuple containing:An array of 2D boolean arrays representing the game fields of multiple games. An array of iteration counts corresponding to each game state.</returns>
        public (bool[][,] fields, int[] iterarions) LoadMultipleGames(string filePath);
    }
}
