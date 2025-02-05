using System;
using System.Collections.Generic;
using GameOfLife.Interfaces;

namespace GameOfLife.Infrastructure
{
    internal class ConsoleRenderer : IRenderer
    {
        public void Render(bool[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

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
