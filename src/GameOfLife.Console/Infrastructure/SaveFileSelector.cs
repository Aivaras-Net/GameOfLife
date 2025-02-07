using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    internal class SaveFileSelector : ISaveFileSelector
    {
        private readonly string saveFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileSelector"/> class.
        /// </summary>
        /// <param name="saveFolder">The directory where saved games are stored.</param>
        public SaveFileSelector(string saveFolder = ConsoleConstants.DefaultSaveFolder)
        {
            this.saveFolder = saveFolder;
        }

        /// <summary>
        /// Retrieves the path to the saved game.
        /// </summary>
        /// <returns>Path to the saved game</returns>
        public string SelectSavedFilePath()
        {
            if (!Directory.Exists(saveFolder))
            {
                Console.WriteLine(ConsoleConstants.NoSaveGamesExistMessage);
                return null;
            }

            string[] files = Directory.GetFiles(saveFolder, ConsoleConstants.SaveFileExtension);
            if (files.Length == 0)
            {
                Console.WriteLine(ConsoleConstants.NoSaveGamesExistMessage);
                return null;
            }

            Console.Clear();
            Console.WriteLine(ConsoleConstants.SelectSavedGameMessage);
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {Path.GetFileName(files[i])}");
            }

            int selection;
            while (true)
            {
                Console.WriteLine(ConsoleConstants.EnterSaveFileNumberMessage);
                string input = Console.ReadLine();
                if (int.TryParse(input, out selection) && selection >= 1 && selection <= files.Length)
                {
                    break;
                }
                Console.WriteLine(ConsoleConstants.InvalidSaveSelectionMessage);
            }
            return files[selection - 1];
        }
    }
}
