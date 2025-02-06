namespace GameOfLife.Core.Infrastucture
{
    /// <summary>
    /// Represents the state of the game for saving and loading.
    /// </summary>
    public class GameState
    {
        public bool[][] Field {  get; set; }
        public int Iteration { get; set; }
    }
}
