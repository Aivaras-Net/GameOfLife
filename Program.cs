namespace GameOfLife
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int fieldSize = DisplayFieldSizeMenu();

            Console.Clear();
            bool[,] field = InitializeField(fieldSize);
            while (true)
            {
                DisplayField(field);
                field = ComputeNextState(field);
                Thread.Sleep(1000);
            }
        }

        static int DisplayFieldSizeMenu()
        {
            string[] options = { "Small (10x10)", "Medium (20x20)", "Large (30x30)", "Custom" };
            int[] presetSizes = { 10, 20, 30 };
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select field size:");
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i == selectedIndex ? ">> " : "   ") + options[i]);
                }

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                }
                else if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex + 1) % options.Length;
                }
                else if (consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
            if (selectedIndex == options.Length - 1)
            {
                int customSize = 0;
                bool valid = false;
                while (!valid)
                {
                    Console.Clear();
                    Console.WriteLine("Enter custom field size (positive integer):");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out customSize) && customSize > 0)
                    {
                        valid = true;
                    }
                }
                return customSize;
            }
            else
            {
                return presetSizes[selectedIndex];
            }

        }

        static bool[,] InitializeField(int fieldSize)
        {
            bool[,] field = new bool[fieldSize, fieldSize];
            Random random = new Random();

            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    field[i, j] = random.Next(2) == 1;
                }
            }
            return field;
        }

        static void DisplayField(bool[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            Console.SetCursorPosition(0, 0);
            DrawHorizontalBorder(cols * 2);

            for (int i = 0; i < rows; i++)
            {
                Console.Write("|");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(field[i, j] ? "░░" : "▓▓"); //Quick solution to make the field look square, whill be changed when implementing multiple gol displays
                }
                Console.WriteLine("|");
            }
            DrawHorizontalBorder(cols * 2);
        }

        private static void DrawHorizontalBorder(int collums)
        {
            Console.Write('+');
            for (int i = 0; i < collums; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine('+');
        }

        static bool[,] ComputeNextState(bool[,] currentField)
        {
            int rows = currentField.GetLength(0);
            int cols = currentField.GetLength(1);

            bool[,] result = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int aliveNeighbors = CountAliveNeighbors(currentField, i, j);
                    if (currentField[i, j])
                    {
                        result[i, j] = (aliveNeighbors == 2 || aliveNeighbors == 3);
                    }
                    else
                    {
                        result[i, j] = (aliveNeighbors == 3);
                    }
                }
            }
            return result;
        }

        static int CountAliveNeighbors(bool[,] field, int row, int col)
        {
            int count = 0;
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i == row && j == col) continue;
                    if ((i >= 0 && i < rows) && (j >= 0 && j < cols) && field[i, j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

    }
}
