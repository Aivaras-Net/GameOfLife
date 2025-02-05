using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Infrastucture
{
    public class FileManager : IFileManager
    {
        public void SaveGame(bool[,] field, string filePath)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            using (StreamWriter writter = new StreamWriter(filePath))
            {
                writter.WriteLine($"{rows},{cols}");
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        writter.Write(field[i, j] ? "1" : "0");
                        if (j < cols - 1)
                        {
                            writter.Write(',');
                        }
                    }
                    writter.WriteLine();
                }
            }
        }
    }
}
