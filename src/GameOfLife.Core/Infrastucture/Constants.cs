namespace GameOfLife.Core.Infrastucture
{
    /// <summary>
    /// Provides constant values used for game logic.
    /// </summary>
    public static class Constants
    {
        public const int DefaultSleepTime = 1000;

        #region File manager constants
        public const string FieldArgumentName = "field";
        public const string DirectoryPathArgumentName = "directoryPath";
        public const string FilePathArgumentName = "filePath";

        public const string NullOrEmptyDirectoryPathMessage = "Directory path cannot be null or empty.";
        public const string NullOrEmptyFilePathMessage = "File path cannot be null or empty.";
        public const string InvalidGameStateDataMessage = "Invalid game state data";

        public const string SingleSaveFilePrefix = "Game";
        public const string SingleFileSearchPattern = SingleSaveFilePrefix + "*.json";

        public const string MultipleSaveFilePrefix = "MultipleGames";
        public const string MultipleSaveFileSearchPattern = MultipleSaveFilePrefix + "*.json";

        public const string SaveFileExtension = ".json";
        public const bool JsonWriteIndented = true;
        #endregion

        public const int Headerheight = 3;
        public const string DefaultSaveFolder = "Saves";
        public const string GameSavedMessage = "Game saved successfully.";
        public const string ExitingGameMessage = "Exiting game...";
        public const string LoadGameFailedMessageFormat = "Failed to lad game : {0}";
    }
}
