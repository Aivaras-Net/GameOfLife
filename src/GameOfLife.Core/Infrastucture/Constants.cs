namespace GameOfLife.Core.Infrastructure
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
        public const string FieldAndIterationMismatchMessage = "Mismatch between fields and iterations length";

        public const string SingleSaveFilePrefix = "Game";
        public const string SingleFileSearchPattern = SingleSaveFilePrefix + "*.json";

        public const string MultipleSaveFilePrefix = "MultipleGames";
        public const string MultipleSaveFileSearchPattern = MultipleSaveFilePrefix + "*.json";

        public const string SaveFileExtension = ".json";
        public const bool JsonWriteIndented = true;
        #endregion

        public const int Headerheight = 3;
        public const string DefaultSaveFolder = "Saves";

        public const string ExitingGameMessage = "Exiting game...";
        public const string GameSavedMessage = "Game saved successfully.";

        public const string AllGamesSavedMessage = "All games saved successfully.";
        public const string SpecificGameSavedMessageFormat = "Game {0} saved successfully.";
        public const string InvalidSaveSelectionMessage = "Invalid selection for saving.";
        public const string InvalidInputMessage = "Invalid input; please enter a number.";

        public const string TogglePauseAllMessage = "Toggled pause state for all games.";
        public const string SpecificGamePauseStateChangedMessageFormat = "Game {0} pause state changed.";
        public const string InvalidTogglePauseSelectionMessage = "Invalid selection for toggling pause state.";
        public const string GamePauseToggledMessage = "Game pause state toggled.";
        public const string ParallelShowcaseNotImplementedMessage = "Parallel Showcase is not implemented yet.";
        public const string NoValidFileSelectedMessage = "No valid file selected.";
        public const string GameLoadedSuccessfullyMessage = "Loaded game(s) successfully.";

        public const string LoadGameFailedMessageFormat = "Failed to lad game : {0}";

        public const string SavePromptFormat = "Enter 0 to save all games in one file or a game number (1-{0}) to save a specific game:";
        public const string TogglePausePromptFormat = "Enter 0 to toggle pause state for all games or a game number (1-{0}) for a specific game:";
    }
}
