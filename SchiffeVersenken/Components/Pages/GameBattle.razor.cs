using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchiffeVersenken.Components.Shared;
using SchiffeVersenken.Data;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Components.Pages
{
    public partial class GameBattle : IDisposable
    {
        // Gets the game logic service instance, cascaded from a parent component
        [CascadingParameter]
        public GameLogicService GameService { get; set; }

        // Background image URL for the ship placement page
        private string bgUrl = "url('../images/backgroundgame.png')";

        // For UI Displayment
        private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private bool playerTurn;

        // GameFields
        private Square[,] _ownfield = null;
        private Square[,] _enemyfield = null;
        // Coordinates selected by the player for their next shot
        private int _currentXCord;
        private int _currentYCord;
        // Game's current score
        private int _score;

        /// <summary>
        /// Initializes game fields and subscribes to game events upon component initialization.
        /// </summary>
		protected override void OnInitialized()
        {
            _ownfield = GameService.Game._BattlefieldPlayer._Board;
            _enemyfield = GameService.Game._BattlefieldOpponent._Board;
            _score = GameService.Game._PlayerScore;
            playerTurn = GameService.Game._Player._YourTurn;

            // Subscribe to game events for real-time UI updates
            GameService.Game._BattlefieldPlayer.OnPlayerAction += UpdateBoards;
            GameService.Game._BattlefieldOpponent.OnPlayerAction += UpdateBoards;
            GameService.Game.OnGameOver += GameOver;

            StateHasChanged();
        }

        /// <summary>
        /// Updates the game board and score whenever a player action occurs.
        /// </summary>
        /// <param name="state">The current state of the square where the action took place.</param>
        public void UpdateBoards(SquareState state) 
        {
			_score = GameService.Game._PlayerScore;
			StateHasChanged();
        }

        /// <summary>
        /// Handles the game over event and navigates back to the lobby.
        /// </summary>
        /// <param name="winner">The winner of the game.</param>
        public async void GameOver(string winner)
        {
			//Show Game Winner
			await OpenDialog(winner);
			StateHasChanged();
			NavigationManager.NavigateTo("/Lobby", true);
		}

        /// <summary>
        /// Captures the coordinates of the game field clicked by the player and shoot if already clicked.
        /// </summary>
        /// <param name="coords">The coordinates of the clicked field.</param>
        public void HandeFieldClicked(int[] coords)
        {
            if (_currentXCord == coords[0] && _currentYCord == coords[1])
            {
                _currentXCord = coords[0];
                _currentYCord = coords[1];
                StateHasChanged();
                OnPlayerShoot();
            }
            _currentXCord = coords[0];
            _currentYCord = coords[1];
        }

        /// <summary>
        /// Processes the player's attempt to shoot at the selected coordinates.
        /// </summary>
        public void OnPlayerShoot()
        {
            SquareState squareState = _enemyfield[_currentXCord, _currentYCord]._State;

			if (squareState == SquareState.Miss ||
				squareState == SquareState.Hit ||
				squareState == SquareState.Sunk)
            {
                OpenAlreadyShotDialog();
			}
            else
            {
				GameService.Game._Player.Shoot(_currentXCord, _currentYCord);
			}
		}

        /// <summary>
        /// Unsubscribes from game events upon component disposal to prevent memory leaks.
        /// </summary>
        public void Dispose()
        {
            GameService.Game._BattlefieldPlayer.OnPlayerAction -= UpdateBoards;
            GameService.Game._BattlefieldOpponent.OnPlayerAction -= UpdateBoards;
            GameService.Game.OnGameOver -= GameOver;
		}

        /// <summary>
        /// Opens a dialog to display the game over message.
        /// </summary>
        /// <param name="winner">The winner of the game.</param>
        /// <returns>A task that represents the asynchronous operation of displaying the dialog.</returns>
        private async Task OpenDialog(string winner)
        {
			DialogParameters parameters = new DialogParameters
			{
				["winner"] = winner,
				["score"] = _score.ToString()
			};
			var dialog = DialogService.Show<GameEndDialog>("Game Over", parameters);
			var result = await dialog.Result;
		}

        /// <summary>
        /// Opens a dialog to inform the player that a field has already been targeted.
        /// </summary>
        private void OpenAlreadyShotDialog()
        {
			DialogParameters parameters = new DialogParameters { { "ContentText", "Field can't be Shoot Again!" } };
			DialogService.Show<ShipPlacementDialog>("", parameters);
		}
	}
}
