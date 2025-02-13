using GameOfLife.Core.Infrastructure;

namespace GameOfLife.Core.Interfaces
{
    public interface IGameInputHandler
    {
        /// <summary>
        /// Retrieves the in game command from the user.
        /// </summary>
        /// <returns>The selected gamme command</returns>
        GameCommand GetCommand();
    }
}
