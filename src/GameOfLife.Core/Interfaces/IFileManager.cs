namespace GameOfLife.Core.Interfaces
{
    public interface IFileManager
    {
        void SaveGame(bool[,] field, int iteration, string directoryPath);

        (bool[,] field, int iteration) LoadGame(string filePath);
    }
}
