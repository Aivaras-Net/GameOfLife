namespace GameOfLife.Interfaces
{
    internal interface IGameLogic
    {
        bool[,] ComputeNextState(bool[,] currentState);
    }
}
