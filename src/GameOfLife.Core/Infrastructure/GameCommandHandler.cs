using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Infrastructure
{
    /// <summary>
    /// Processes game commands and delegates saving and pause actions.
    /// </summary>
    public class GameCommandHandler : IGameCommandHandler
    {
        private readonly IGameInputHandler _gameInputHandler;
        private readonly IRenderer _renderer;

        public GameCommandHandler(IGameInputHandler gameInputHandler, IRenderer renderer)
        {
            _gameInputHandler = gameInputHandler;
            _renderer = renderer;
        }

        /// <summary>
        /// Processes a game command and triggers the corresponding actions.
        /// </summary>
        /// <param name="numberOfGames">The number of active games.</param>
        /// <param name="onSaveAll">Action to save all games.</param>
        /// <param name="onSaveSingle">Action to save a single game, given its index.</param>
        /// <param name="onTogglePauseAll">Action to toggle pause for all games.</param>
        /// <param name="onTogglePauseSingle">Action to toggle pause for a single game, given its index.</param>
        /// <param name="onViewGame">Action to view a game, given its index.</param>
        /// <returns>False if the command is to quit; otherwise, true.</returns>
        public bool ProcessCommand(
            int numberOfGames,
            Action onSaveAll,
            Action<int> onSaveSingle,
            Action onTogglePauseAll,
            Action<int> onTogglePauseSingle,
            Action<int> onViewGame = null)
        {
            GameCommand command = _gameInputHandler.GetCommand();
            switch (command)
            {
                case GameCommand.Quit:
                    _renderer.RenderMessage(Constants.ExitingGameMessage);
                    _renderer.Flush();
                    return false;

                case GameCommand.Save:
                    HandleSaveCommand(numberOfGames, onSaveAll, onSaveSingle);
                    break;

                case GameCommand.Stop:
                    HandleStopCommand(numberOfGames, onTogglePauseAll, onTogglePauseSingle);
                    break;

                case GameCommand.View when numberOfGames == 1000 && onViewGame != null:
                    HandleViewCommand(numberOfGames, onViewGame);
                    break;

                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Handles the save command for single or multiple games.
        /// </summary>
        /// <param name="numberOfGames">The number of active games.</param>
        /// <param name="onSaveAll">Action to save all games.</param>
        /// <param name="onSaveSingle">Action to save a single game, given its index.</param>
        private void HandleSaveCommand(int numberOfGames, Action onSaveAll, Action<int> onSaveSingle)
        {
            if (numberOfGames > 1)
            {
                string prompt = string.Format(Constants.SavePromptFormat, numberOfGames);
                string input = _renderer.Prompt(prompt);
                if (int.TryParse(input, out int selection))
                {
                    if (selection == 0)
                    {
                        onSaveAll();
                        _renderer.RenderMessage(Constants.AllGamesSavedMessage);
                    }
                    else if (selection >= 1 && selection <= numberOfGames)
                    {
                        onSaveSingle(selection - 1);
                        _renderer.RenderMessage(string.Format(Constants.SpecificGameSavedMessageFormat, selection));
                    }
                    else
                    {
                        _renderer.RenderMessage(Constants.InvalidSaveSelectionMessage);
                    }
                }
                else
                {
                    _renderer.RenderMessage(Constants.InvalidInputMessage);
                }
            }
            else
            {
                onSaveSingle(0);
                _renderer.RenderMessage(Constants.GameSavedMessage);
            }
        }

        /// <summary>
        /// Handles the pause toggle command for single or multiple games.
        /// </summary>
        /// <param name="numberOfGames">The number of active games.</param>
        /// <param name="onTogglePauseAll">Action to toggle pause for all games.</param>
        /// <param name="onTogglePauseSingle">Action to toggle pause for a single game, given its index.</param>
        private void HandleStopCommand(int numberOfGames, Action onTogglePauseAll, Action<int> onTogglePauseSingle)
        {
            if (numberOfGames > 1)
            {
                string prompt = string.Format(Constants.TogglePausePromptFormat, numberOfGames);
                string input = _renderer.Prompt(prompt);
                if (int.TryParse(input, out int selection))
                {
                    if (selection == 0)
                    {
                        onTogglePauseAll();
                        _renderer.RenderMessage(Constants.TogglePauseAllMessage);
                    }
                    else if (selection >= 1 && selection <= numberOfGames)
                    {
                        onTogglePauseSingle(selection - 1);
                        _renderer.RenderMessage(string.Format(Constants.SpecificGamePauseStateChangedMessageFormat, selection));
                    }
                    else
                    {
                        _renderer.RenderMessage(Constants.InvalidTogglePauseSelectionMessage);
                    }
                }
                else
                {
                    _renderer.RenderMessage(Constants.InvalidInputMessage);
                }
            }
            else
            {
                onTogglePauseSingle(0);
                _renderer.RenderMessage(Constants.GamePauseToggledMessage);
            }
        }

        private void HandleViewCommand(int numberOfGames, Action<int> onViewGame)
        {
            string prompt = string.Format(Constants.ViewGamePromptFormat, numberOfGames);
            string input = _renderer.Prompt(prompt);
            if (int.TryParse(input, out int selection) && selection >= 1 && selection <= numberOfGames)
            {
                onViewGame(selection - 1);
                _renderer.RenderMessage(string.Format(Constants.ViewGameChangedMessageFormat, selection));
            }
            else
            {
                _renderer.RenderMessage(Constants.InvalidViewSelectionMessage);
            }
        }
    }
}
