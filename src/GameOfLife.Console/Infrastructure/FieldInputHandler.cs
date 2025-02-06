using GameOfLife.Core.Infrastucture;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    /// <summary>
    /// Manages user input for the Game of life.
    /// </summary>
    internal class FieldInputHandler : IInputHandler
    {
        /// <summary>
        /// Prompts the user to select or enter a field size.
        /// </summary>
        /// <returns>A positive integer representing the game field size.</returns>
        public int GetFieldSize()
        {
            int[] presetSizes = ConsoleConstants.PresetFieldSizes;
            string[] options = presetSizes.Select(size => $"{size}x{size}").Concat(new[] { ConsoleConstants.CustomOption }).ToArray();
            int selectedIndex = 0;

            // Loop until user makes a selection.
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ConsoleConstants.SelectFieldSizeMessage);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i == selectedIndex ? ConsoleConstants.ArrowPointer : ConsoleConstants.NoArrowPrefix) + options[i]);
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
                Console.WriteLine(ConsoleConstants.CustomFieldSizePromt);
                if (int.TryParse(Console.ReadLine(), out customSize) && customSize > 0)
                {
                    Console.Clear();
                    return customSize;
                }

            }
        }

        public GameStartMode GetGameStartMode()
        {
            Console.Clear();
            Console.WriteLine("Select an option");
            Console.WriteLine("L: Load game");
            Console.WriteLine("N: New game");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.L)
            {
                return GameStartMode.Load;
            }
            else
            {
                return GameStartMode.New;
            }
        }

        public string GetSavedFilePath()
        {
            string saveFolder = "Saves";
            if (!Directory.Exists(saveFolder))
            {
                Console.WriteLine("No save games exist");
                return null;
            }

            string[] files = Directory.GetFiles(saveFolder, "*.json");
            if (files.Length == 0)
            {
                Console.WriteLine("No saved games exits");
                return null;
            }

            Console.Clear();
            Console.WriteLine("Select a saved game to load:");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {Path.GetFileName(files[i])}");
            }

            int selection = 0;
            while (true)
            {
                Console.WriteLine("Enter the number of the save file:");
                string input = Console.ReadLine();
                if(int.TryParse (input, out selection) && selection >=1 && selection <=files.Length)
                {
                    break;
                }
                Console.WriteLine("Invalid selection. Try again");
            }
            return files[selection - 1];
        }
    }
}
