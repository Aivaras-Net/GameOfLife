namespace GameOfLife.Interfaces
{
    internal interface IGameLogic
    {
        /// <summary>
        /// Computes the next state of the game from current configuration.
        /// </summary>
        /// <param name="currentState">A two dimentional boolean array representing the current state of the cells.</param>
        /// <returns>A two dimentional boolean array representing the subsequent state of the cells.</returns>
        bool[,] ComputeNextState(bool[,] currentState);
    }
}
