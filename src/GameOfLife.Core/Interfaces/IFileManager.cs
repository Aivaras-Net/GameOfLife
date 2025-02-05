namespace GameOfLife.Core.Interfaces
{
    public interface IFileManager
    {
        void SaveGame(bool[,] field, string filePath);
    }
}
