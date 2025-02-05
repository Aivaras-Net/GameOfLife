using System;
using System.Collections.Generic;
using GameOfLife.Interfaces;

namespace GameOfLife.Infrastructure
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
            Console.SetCursorPosition(Constants.ConsoleCursorPositionX, Constants.ConsoleCursorPositionY);
            DrawHorizontalBorder(cols * Constants.CellWidthMultiplier);

            for (int i = 0; i < rows; i++)
            {
                Console.Write(Constants.BorderVertical);
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(field[i, j] ? Constants.AliveCellSymbol : Constants.DeadCellSymbol); //Quick solution to make the field look square, whill be changed when implementing multiple gol displays
                }
                Console.WriteLine(Constants.BorderVertical);
            }
            DrawHorizontalBorder(cols * Constants.CellWidthMultiplier);
        }

        /// <summary>
        /// Draws a horizontal border line for the game field.
        /// </summary>
        /// <param name="columns">Number of columns.</param>
        private static void DrawHorizontalBorder(int columns)
        {
            Console.Write(Constants.BorderCorner);
            for (int i = 0; i < columns; i++)
            {
                Console.Write(Constants.BorderHorizontal);
            }
            Console.WriteLine(Constants.BorderCorner);
        }
    }
}
