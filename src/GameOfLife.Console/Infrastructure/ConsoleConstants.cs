namespace GameOfLife.CLI.Infrastructure
{
    /// <summary>
    /// Provides constant values used for console implementation
    /// </summary>
    public static class ConsoleConstants
    {
        public const char AliveCellSymbol = '░';
        public const char DeadCellSymbol = '▓';
        public const char BorderVertical = '|';
        public const char BorderHorizontal = '-';
        public const char BorderCorner = '+';
        public const char EmptyBufferFill = ' ';
        public const int CellWidthMultiplier = 1;
        public const int ConsoleCursorPositionX = 0;
        public const int ConsoleCursorPositionY = 0;

        public const string Header = "I-current iteration; LC- the number of living cells";
        public const string CommandGuide = "Press: S - Save ; P - Pause; Q - Exit";
        public const string TruncationMessage = "Output truncated. Select smaller fields or use bigger output window.";
        public const string ConcurentGameNumberPrompt = "Enter number of games to show (1-8): ";
        public static readonly int[] PresetFieldSizes = { 10, 20, 30 };
        public const string SelectFieldSizeMessage = "Select field size:";
        public const string CustomOption = "Custom";
        public const string CustomFieldSizePromt = "Enter custom field size (positive integer):";
        public const string ArrowPointer = ">> ";
        public const string NoArrowPrefix = "    ";

        public const string GameStartModePromptMessage = "Select an option:";
        public const string LoadGameOptionMessage = "L: Load game";
        public const string NewGameOptionMessage = "N: New game";
        public const string ParallelShowcaseOptionMessage = "P: Parallel Showcase";
        public const string NoSaveGamesExistMessage = "No save games exist.";
        public const string SelectSavedGameMessage = "Select a saved game to load:";
        public const string EnterSaveFileNumberMessage = "Enter the number of the save file:";
        public const string InvalidSaveSelectionMessage = "Invalid selection. Try again.";
        public const string GameSavedSuccessMessage = "Game saved successfully.";
        public const string ExitingGameMessage = "Exiting game...";
        public const string FailedToLoadGameMessage = "Failed to load game: {0}";
        public const string GameStatisticsFormat = "ID:{0}|I:{1}|LC:{2}";

        public const string DefaultSaveFolder = "Saves";
        public const string SaveFileExtension = "*.json";
    }
}
