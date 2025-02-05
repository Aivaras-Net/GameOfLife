﻿using GameOfLife.Interfaces;

namespace GameOfLife.Infrastructure
{
    /// <summary>
    /// Manages user input for the Game of life.
    /// </summary>
    internal class UserInputHandler : IInputHandler
    {
        /// <summary>
        /// Prompts the user to select or enter a field size.
        /// </summary>
        /// <returns>A positive integer representing the game field size.</returns>
        public int GetFieldSize()
        {
            int[] presetSizes = Constants.PresetFieldSizes;
            string[] options = presetSizes.Select(size => $"{size}x{size}").Concat(new[] {Constants.CustomOption}).ToArray();
            int selectedIndex = 0;

            // Loop until user makes a selection.
            while (true)
            {
                Console.Clear();
                Console.WriteLine(Constants.SelectFieldSizeMessage);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i == selectedIndex ? Constants.ArrowPointer : Constants.NoArrowPrefix) + options[i]);
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                    selectedIndex = (selectedIndex + 1) % options.Length;
                else if (keyInfo.Key == ConsoleKey.Enter)
                    break;
            }

            return selectedIndex == options.Length - 1 ? GetCustomSize() : presetSizes[selectedIndex];

        }

        /// <summary>
        /// Prompts the user to enter a custom game field size.
        /// </summary>
        /// <returns>A positive integer representing the game field size.</returns>
        private static int GetCustomSize()
        {
            int customSize;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(Constants.CustomFieldSizePromt);
                if (int.TryParse(Console.ReadLine(), out customSize) && customSize > 0)
                {
                    Console.Clear();
                    return customSize;
                }

            }
        }
    }
}
