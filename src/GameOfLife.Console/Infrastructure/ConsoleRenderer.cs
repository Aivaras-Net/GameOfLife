using System.Text;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    /// <summary>
    /// Responsible for buffering and flushing console output.
    /// </summary>
    internal class ConsoleRenderer : IRenderer
    {
        private char[,] _screenBuffer;
        private int _bufferWidth;
        private int _bufferHeight;
        private bool _outputTruncated;

        /// <summary>
        /// Initializes the off–screen buffer for the current frame.
        /// </summary>
        public void BeginFrame()
        {
            _bufferWidth = Console.WindowWidth;
            _bufferHeight = Console.WindowHeight;
            _outputTruncated = false;
            _screenBuffer = new char[_bufferHeight, _bufferWidth];

            // Fill buffer with spaces.
            for (int i = 0; i < _bufferHeight; i++)
            {
                for (int j = 0; j < _bufferWidth; j++)
                {
                    _screenBuffer[i, j] = ConsoleConstants.EmptyBufferFill;
                }
            }

            DrawString(ConsoleConstants.Header, 0, 0);
            DrawString(ConsoleConstants.CommandGuide, 0, 1);
        }

        /// <summary>
        /// Draws a string into the off–screen buffer at the given coordinates.
        /// </summary>
        private void DrawString(string text, int x, int y)
        {
            if (y < 0 || y >= _bufferHeight)
            {
                _outputTruncated = true;
                return;
            }

            if (x < 0 || x + text.Length > _bufferWidth)
            {
                _outputTruncated = true;
            }

            if (y < 0 || y >= _bufferHeight) return;

            for (int i = 0; i < text.Length; i++)
            {
                int col = x + i;
                if (col < 0 || col >= _bufferWidth) continue;
                _screenBuffer[y, col] = text[i];
            }
        }

        /// <summary>
        /// Flushes the off–screen buffer to the console in one update.
        /// </summary>
        public void Flush()
        {
            if (_outputTruncated)
            {
                string truncMessage = ConsoleConstants.TruncationMessage;
                string paddedMessage = truncMessage.PadRight(_bufferWidth);
                for (int j = 0; j < _bufferWidth; j++)
                {
                    _screenBuffer[_bufferHeight - 1, j] = paddedMessage[j];
                }
            }

            Console.SetCursorPosition(0, 0);
            var sb = new StringBuilder();
            for (int i = 0; i < _bufferHeight; i++)
            {
                for (int j = 0; j < _bufferWidth; j++)
                {
                    sb.Append(_screenBuffer[i, j]);
                }
                if (i < _bufferHeight - 1)
                    sb.AppendLine();
            }
            Console.Write(sb.ToString());
        }

        /// <summary>
        /// Renders a game field along with statistics and border.
        /// </summary>
        /// <param name="field">2D boolean array representing the game field.</param>
        /// <param name="iteration">Current iteration number.</param>
        /// <param name="livingCells">Number of living cells.</param>
        /// <param name="offsetX">X coordinate offset for rendering.</param>
        /// <param name="offsetY">Y coordinate offset for rendering.</param>
        public void Render(bool[,] field,
                           int gameId,
                           int iteration,
                           int livingCells,
                           bool paused = false,
                           int offsetX = ConsoleConstants.ConsoleCursorPositionX,
                           int offsetY = ConsoleConstants.ConsoleCursorPositionY
                           )
        {
            // Render statistics.
            string stats = string.Format(ConsoleConstants.GameStatisticsFormat, gameId, iteration, livingCells);
            if (paused)
            {
                stats += ConsoleConstants.PausedStateMessage;
            }
            DrawString(stats, offsetX, offsetY);

            int rows = field.GetLength(0);
            int cols = field.GetLength(1);
            int fieldStartX = offsetX;
            int fieldStartY = offsetY + 1;

            // Prepare horizontal border string.
            string horizontalBorder = ConsoleConstants.BorderCorner +
                                      new string(ConsoleConstants.BorderHorizontal, cols * ConsoleConstants.CellWidthMultiplier) +
                                      ConsoleConstants.BorderCorner;
            // Top border.
            DrawString(horizontalBorder, fieldStartX, fieldStartY);

            // Field rows with vertical borders.
            for (int i = 0; i < rows; i++)
            {
                var rowBuilder = new StringBuilder();
                rowBuilder.Append(ConsoleConstants.BorderVertical);
                for (int j = 0; j < cols; j++)
                {
                    rowBuilder.Append(field[i, j] ? ConsoleConstants.AliveCellSymbol : ConsoleConstants.DeadCellSymbol);
                }
                rowBuilder.Append(ConsoleConstants.BorderVertical);
                DrawString(rowBuilder.ToString(), fieldStartX, fieldStartY + 1 + i);
            }

            // Bottom border.
            DrawString(horizontalBorder, fieldStartX, fieldStartY + rows + 1);
        }

        /// <summary>
        /// Renders a temporary message at the bottom of the console.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void RenderMessage(string message)
        {
            int messageY = Console.WindowHeight - 1;
            // Pad the message to clear the line.
            DrawString(message.PadRight(Console.WindowWidth), 0, messageY);
        }

        /// <summary>
        /// Displays a prompt message and returns user input.
        /// </summary>
        /// <param name="message">The prompt message to display.</param>
        /// <returns>User input as a string.</returns>
        public string Prompt(string message)
        {
            RenderMessage(message);
            Flush();
            return Console.ReadLine();
        }

        /// <summary>
        /// Renders global statistics about all games.
        /// </summary>
        /// <param name="activeGames">Number of active (non-paused) games.</param>
        /// <param name="totalLivingCells">Total number of living cells across all games.</param>
        public void RenderGlobalStats(int activeGames, int totalLivingCells)
        {
            string stats = string.Format(ConsoleConstants.GlobalStatisticsFormat, activeGames, totalLivingCells);
            DrawString(stats, 0, 2);
        }
    }
}
