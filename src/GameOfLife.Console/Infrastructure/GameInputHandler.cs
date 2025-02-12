using GameOfLife.Core.Infrastructure;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    /// <summary>
    /// Handles in-game input commands.
    /// </summary>
    internal class GameInputHandler : IGameInputHandler
    {
        /// <summary>
        /// Retrieves a command from the user input.
        /// </summary>
        /// <returns></returns>
        public GameCommand GetCommand()
        {
            if (!Console.KeyAvailable)
                return GameCommand.None;

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.S:
                    return GameCommand.Save;
                case ConsoleKey.P:
                    return GameCommand.Stop;
                case ConsoleKey.Q:
                    return GameCommand.Quit;
                default:
                    return GameCommand.None;
            }
        }
    }
}
