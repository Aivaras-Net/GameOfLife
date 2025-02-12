namespace GameOfLife.Core.Infrastructure
{
    /// <summary>
    /// Represents the command that can be executed during the game.
    /// </summary>
    public enum GameCommand
    {
        None,
        Save,
        Stop,
        Quit
    }
}
