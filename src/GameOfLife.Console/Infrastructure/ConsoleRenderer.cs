using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    /// <summary>
    /// Responsible for the console output.
    /// </summary>
    internal class ConsoleRenderer : IRenderer
    {
        /// <summary>
        /// Renders the game field, iteration count, and living cells statistics.
        /// </summary>
        /// <param name="field">2D boolean array representing the game field.<</param>
        /// <param name="iteration">Current iteration number.</param>
        /// <param name="livingCells">Number of living cells.</param>
        /// <param name="offsetX">X coordinate offset for rendering.</param>
        /// <param name="offsetY">Y coordinate offset for rendering.</param>
        public void Render(bool[,] field, int iteration, int livingCells, int offsetX = ConsoleConstants.ConsoleCursorPositionX, int offsetY = ConsoleConstants.ConsoleCursorPositionY)
        {
            Console.SetCursorPosition(offsetX, offsetY);
            Console.WriteLine(ConsoleConstants.CommandGuide);

            RenderStatistics(iteration, livingCells, offsetX, offsetY + 1);

            int rows = field.GetLength(0);
            int cols = field.GetLength(1);
            int fieldStartX = offsetX;
            int fieldStartY = offsetY + 2;

            DrawHorizontalBorder(cols * ConsoleConstants.CellWidthMultiplier, fieldStartX, fieldStartY);

            for (int i = 0; i < rows; i++)
            {
                Console.SetCursorPosition(fieldStartX, fieldStartY + 1 + i);
                Console.Write(ConsoleConstants.BorderVertical);
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(field[i, j] ? ConsoleConstants.AliveCellSymbol : ConsoleConstants.DeadCellSymbol);
                }
                Console.Write(ConsoleConstants.BorderVertical);
            }

            Console.SetCursorPosition(fieldStartX, fieldStartY + rows + 1);
            DrawHorizontalBorder(cols * ConsoleConstants.CellWidthMultiplier, fieldStartX, fieldStartY + rows + 1);
        }

        /// <summary>
        /// Renders the game statistics above the field.
        /// </summary>
        /// <param name="iteration">Current iteration number.</param>
        /// <param name="livingCells">Number of living cells.</param>
        /// <param name="offsetX">Left offset.</param>
        /// <param name="offsetY">Top offset (statistics will be rendered at this Y coordinate).</param>
        public void RenderStatistics(int iteration, int livingCells, int offsetX, int offsetY)
        {
            Console.SetCursorPosition(offsetX, offsetY);
            Console.WriteLine($"Iteration {iteration} | Living Cells: {livingCells}");
        }

        /// <summary>
        /// Draws a horizontal border line for the game field.
        /// </summary>
        /// <param name="width">Width of the border in characters.</param>
        /// <param name="offsetX">Left offset.</param>
        /// <param name="offsetY">Y coordinate for the border.</param>
        private static void DrawHorizontalBorder(int width, int offsetX, int offsetY)
        {
            Console.SetCursorPosition(offsetX, offsetY);
            Console.Write(ConsoleConstants.BorderCorner);
            for (int i = 0; i < width; i++)
            {
                Console.Write(ConsoleConstants.BorderHorizontal);
            }
            Console.WriteLine(ConsoleConstants.BorderCorner);
        }

        /// <summary>
        /// Renders a temporary message at the bottom of the console.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void RenderMessage(string message)
        {
            int messageY = Console.WindowHeight - 1;
            Console.SetCursorPosition(0, messageY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, messageY);
            Console.WriteLine(message);
        }
    }
}
