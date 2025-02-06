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
    }
}
