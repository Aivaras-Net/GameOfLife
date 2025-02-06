using GameOfLife.Core.Infrastucture;

namespace GameOfLife.Core.Interfaces
{
    public interface IInputHandler
    {
        /// <summary>
        /// Retrieves the field size from the user.
        /// </summary>
        /// <returns>A positive integer representing the dimentions of a square game field.</returns>
        int GetFieldSize();

        /// <summary>
        /// Retrieves the game start mode from the user.
        /// </summary>
        /// <returns></returns>
        GameStartMode GetGameStartMode();


        /// <summary>
        /// Retrieves the saved game file path from the user.
        /// </summary>
        /// <returns></returns>
        string GetSavedFilePath();
    }
}
