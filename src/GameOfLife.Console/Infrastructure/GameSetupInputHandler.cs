using GameOfLife.Core.Infrastucture;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    /// <summary>
    /// Manages user input for starting the Game of life.
    /// </summary>
    internal class GameSetupInputHandler : IGameSetupInputHandler 
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

        /// <summary>
        /// Prompts the user to select the game start mode (Load or New game).
        /// </summary>
        /// <returns>The selected mode.</returns>
        public GameStartMode GetGameStartMode()
        {
            Console.Clear();
            Console.WriteLine(ConsoleConstants.GameStartModePromptMessage);
            Console.WriteLine(ConsoleConstants.LoadGameOptionMessage);
            Console.WriteLine(ConsoleConstants.NewGameOptionMessage);
            Console.WriteLine(ConsoleConstants.ParallelShowcaseOptionMessage);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.L:
                    return GameStartMode.Load;
                case ConsoleKey.N:
                    return GameStartMode.New;
                case ConsoleKey.P:
                    return GameStartMode.ParralelShowcase;
                default:
                    return GameStartMode.New;
            }
        }

        /// <summary>
        /// Prompts the user to select number of games to show on the screen.
        /// </summary>
        /// <returns>Number of games.</returns>
        public int GetNumberOfGames()
        {
            int numberOfGames;
            do
            {
                Console.Clear();
                Console.WriteLine(ConsoleConstants.ConcurentGameNumberPrompt);
            }
            while (!int.TryParse(Console.ReadLine(), out numberOfGames));
            return numberOfGames;
        }

        /// <summary>
        /// Prompts the user to select number of games to show on the screen.
        /// </summary>
        /// <returns>Number of games.</returns>
        public int GetNumberOfGames()
        {
            int numberOfGames;
            do
            {
                Console.Clear();
                Console.WriteLine(ConsoleConstants.ConcurentGameNumberPrompt);
            }
            while (!int.TryParse(Console.ReadLine(), out numberOfGames));
            return numberOfGames;
        }
    }
}
