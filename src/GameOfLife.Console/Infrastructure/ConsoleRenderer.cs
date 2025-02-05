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

        private static void DrawHorizontalBorder(int columns)
        {
            Console.Write('+');
            for (int i = 0; i < columns; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine('+');
        }
    }
}
