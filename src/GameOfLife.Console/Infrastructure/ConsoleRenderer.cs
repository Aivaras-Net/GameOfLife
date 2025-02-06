using System;
using System.Collections.Generic;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    /// <summary>
    /// Responsible for the console output.
    /// </summary>
    internal class ConsoleRenderer : IRenderer
    {
        /// <summary>
        /// Renders the current state of the game.
        /// </summary>
        /// <param name="field">Two dimentional boolean array representing alive and dead cells.</param>
        public void Render(bool[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            //Reset the cursor to the top left to avoid flickering from updates.
            Console.SetCursorPosition(ConsoleConstants.ConsoleCursorPositionX, ConsoleConstants.ConsoleCursorPositionY);
            Console.WriteLine("Press S to Save, Q to Quit");

            int startY= ConsoleConstants.ConsoleCursorPositionY +1;

            Console.SetCursorPosition(ConsoleConstants.ConsoleCursorPositionX, startY);
            DrawHorizontalBorder(cols * ConsoleConstants.CellWidthMultiplier);

            for (int i = 0; i < rows; i++)
            {
                Console.Write(ConsoleConstants.BorderVertical);
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(field[i, j] ? ConsoleConstants.AliveCellSymbol : ConsoleConstants.DeadCellSymbol); //Quick solution to make the field look square, whill be changed when implementing multiple gol displays
                }
                Console.WriteLine(ConsoleConstants.BorderVertical);
            }
            DrawHorizontalBorder(cols * ConsoleConstants.CellWidthMultiplier);
        }

        /// <summary>
        /// Draws a horizontal border line for the game field.
        /// </summary>
        /// <param name="columns">Number of columns.</param>
        private static void DrawHorizontalBorder(int columns)
        {
            Console.Write(ConsoleConstants.BorderCorner);
            for (int i = 0; i < columns; i++)
            {
                Console.Write(ConsoleConstants.BorderHorizontal);
            }
            Console.WriteLine(ConsoleConstants.BorderCorner);
        }

        public void RenderStatistics (int iteration, int livingCells, int fieldHeight)
        {
            int y = ConsoleConstants.ConsoleCursorPositionY + fieldHeight + 2;
            Console.SetCursorPosition(0,y);

            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, y);

            Console.WriteLine($"Iteration {iteration} | Living Cells: {livingCells}");
        }

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
