namespace GameOfLife.Core.Models
{
    /// <summary>
    /// Represents a collection of game states for serialization.
    /// </summary>
    public class CombinedGameState
    {
        public GameStateData[] GameStates { get; set; }
    }
}