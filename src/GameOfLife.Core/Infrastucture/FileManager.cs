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

            //using (StreamWriter writter = new StreamWriter(filePath))
            //{
            //    writter.WriteLine($"{rows},{cols}");
            //    for (int i = 0; i < rows; i++)
            //    {
            //        for (int j = 0; j < cols; j++)
            //        {
            //            writter.Write(field[i, j] ? "1" : "0");
            //            if (j < cols - 1)
            //            {
            //                writter.Write(',');
            //            }
            //        }
            //        writter.WriteLine();
            //    }
            //}

            using (var stream = File.Open(filePath, FileMode.Create))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(rows);
                writer.Write(cols);

                byte currentByte = 0;
                int bitCount = 0;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        currentByte = (byte)(currentByte << 1);
                        if (field[i, j])
                        {
                            currentByte |= 1;
                        }
                        bitCount++;

                        if (bitCount == 8)
                        {
                            writer.Write(currentByte);
                            currentByte = 0;
                            bitCount = 0;
                        }
                    }
                }

                if (bitCount > 0)
                {
                    currentByte = (byte)(currentByte << (8 - bitCount));
                    writer.Write(currentByte);
                }
            }
            }
        public bool[,] LoadGame(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("File path cannot be null or empty.", nameof(filePath));
            }

            using (var stream = File.Open(filePath, FileMode.Open))
            using (var reader = new BinaryReader(stream))
            {
                int rows = reader.ReadInt32();
                int cols = reader.ReadInt32();
                bool[,] field = new bool[rows, cols];

                int totalCells = rows * cols;
                int cellIndex = 0;

                while (cellIndex < totalCells)
                {
                    byte currentByte = reader.ReadByte();
                    int bitsInByte = Math.Min(8,totalCells - cellIndex);
                    for (int bit = bitsInByte - 1; bit >= 0; bit--)
                    {
                        bool cellValue = ((currentByte >> bit) & 1) ==1;
                        int row = cellIndex/ cols;
                        int col = cellIndex% rows;
                        field[row, col] = cellValue;
                        cellIndex++;
                    }
                }
                return field;
            }

        }
    }
}

