using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Core.Infrastucture;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.CLI.Infrastructure
{
    internal class GameInputHandler : IGameInputHandler
    {
        public GameCommand GetCommand()
        {
            if (!Console.KeyAvailable)
            {
                return GameCommand.None;
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.S:
                    return GameCommand.Save;
                case ConsoleKey.Q:
                    return GameCommand.Quit;
                default:
                    return GameCommand.None;
            }
        }
    }
}
