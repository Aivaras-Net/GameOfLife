using GameOfLife.Core.Infrastructure;
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
            string[] options = presetSizes.Select(size => $"{size}x{size}")
                                        .Concat(new[] { ConsoleConstants.CustomOption })
                                        .ToArray();

            int selectedIndex = ConsoleSelectionUtility.GetSelectionFromOptions(
                ConsoleConstants.SelectFieldSizeMessage,
                options);

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
                Console.WriteLine(ConsoleConstants.CustomFieldSizePrompt);
                if (int.TryParse(Console.ReadLine(), out customSize) && customSize > 0)
                {
                    Console.Clear();
                    return customSize;
                }

                Console.WriteLine(ConsoleConstants.InvalidFieldSizeMessage);
                Thread.Sleep(ConsoleConstants.MessageSleepTime);
            }
        }

        /// <summary>
        /// Prompts the user to select the game start mode (Load or New game).
        /// </summary>
        /// <returns>The selected mode.</returns>
        public GameStartMode GetGameStartMode()
        {
            string[] options = {
                ConsoleConstants.LoadGameOptionMessage,
                ConsoleConstants.NewGameOptionMessage,
                ConsoleConstants.ParallelShowcaseOptionMessage
            };

            int selectedIndex = ConsoleSelectionUtility.GetSelectionFromOptions(
                ConsoleConstants.GameStartModePromptMessage,
                options);

            return selectedIndex switch
            {
                0 => GameStartMode.Load,
                1 => GameStartMode.New,
                2 => GameStartMode.ParralelShowcase,
                _ => GameStartMode.New
            };
        }

        /// <summary>
        /// Prompts the user to select number of games to show on the screen.
        /// </summary>
        /// <returns>Number of games.</returns>
        public int GetNumberOfGames()
        {
            int numberOfGames;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ConsoleConstants.ConcurentGameNumberPrompt);

                if (int.TryParse(Console.ReadLine(), out numberOfGames) &&
                    numberOfGames >= ConsoleConstants.MIN_GAMES &&
                    numberOfGames <= ConsoleConstants.MAX_GAMES)
                {
                    return numberOfGames;
                }

                Console.WriteLine(ConsoleConstants.InvalidNumberOfGamesMessage);
                Thread.Sleep(ConsoleConstants.MessageSleepTime);
            }
        }
    }
}
