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

        GameStartMode GetGameStartMode();

        string GetSavedFilePath();
    }
}
