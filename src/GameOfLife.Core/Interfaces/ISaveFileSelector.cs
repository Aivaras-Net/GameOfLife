namespace GameOfLife.Core.Interfaces
{
    public interface ISaveFileSelector
    {
        /// <summary>
        /// Retrieves the path to the saved game.
        /// </summary>
        /// <returns>Path to the save game</returns>
        string SelectSavedFilePath();
    }
}
