using GameOfLife.Core.Infrastucture;

namespace GameOfLife.Core.Interfaces
{
    public interface IGameInputHandler
    {
        GameCommand GetCommand();
    }
}
