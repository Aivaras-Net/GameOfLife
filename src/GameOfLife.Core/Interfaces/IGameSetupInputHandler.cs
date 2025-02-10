using GameOfLife.Core.Infrastucture;

namespace GameOfLife.Core.Interfaces
{
    public interface IGameSetupInputHandler
    {
        /// <summary>
        /// Retrieves the field size from the user.
        /// </summary>
        /// <returns>A positive integer representing the dimentions of a square game field.</returns>
        int GetFieldSize();

        /// <summary>
        /// Retrieves the game start mode from the user.
        /// </summary>
        /// <returns>Selected game start mode</returns>
        GameStartMode GetGameStartMode();

        /// <summary>
        /// Retrieves the number of games to show at once.
        /// </summary>
        /// <returns>Number of games</returns>
        public int GetNumberOfGames();
    }
}
