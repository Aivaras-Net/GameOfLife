namespace GameOfLife.CLI.Infrastructure
{
    /// <summary>
    /// Provides constant values used for console implementation
    /// </summary>
    public static class ConsoleConstants
    {
        public const string AliveCellSymbol = "░░";
        public const string DeadCellSymbol = "▓▓";
        public const string BorderVertical = "|";
        public const string BorderHorizontal = "-";
        public const string BorderCorner = "+";
        public const int CellWidthMultiplier = 2;
        public const int ConsoleCursorPositionX = 0;
        public const int ConsoleCursorPositionY = 0;

        public static readonly int[] PresetFieldSizes = { 10, 20, 30 };
        public const string SelectFieldSizeMessage = "Select field size:";
        public const string CustomOption = "Custom";
        public const string CustomFieldSizePromt = "Enter custom field size (positive integer):";
        public const string ArrowPointer = ">> ";
        public const string NoArrowPrefix = "    ";
    }
}
