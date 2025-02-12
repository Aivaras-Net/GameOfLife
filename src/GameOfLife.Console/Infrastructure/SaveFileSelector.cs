using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    internal class SaveFileSelector : ISaveFileSelector
    {
        private readonly string saveFolder;

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

            string[] options = files.Select(Path.GetFileName).ToArray();

            int selection = ConsoleSelectionUtility.GetSelectionFromOptions(
                ConsoleConstants.SelectSavedGameMessage,
                options);

            return files[selection];
        }
    }
}
