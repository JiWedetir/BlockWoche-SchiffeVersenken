using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchiffeVersenken.Components.Shared;
using SchiffeVersenken.Data;
using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Components.Pages
{
	public partial class GameBattle : IDisposable
    {
		[CascadingParameter]
        public GameLogicService GameService { get; set; }

        private char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private Square[,] _ownfield = null;
        private Square[,] _enemyfield = null;
        private bool _playerTurn;
        private int _currentXCord;
        private int _currentYCord;
        private int _score;
        private SquareState _state;
		private string _bgUrl = "url('../images/backgroundgame.png')";

		protected override void OnInitialized()
        {
            _ownfield = GameService.Game._BattlefieldPlayer._Board;
            _enemyfield = GameService.Game._BattlefieldOpponent._Board;
            _playerTurn = GameService.Game._Player._YourTurn;
            _score = GameService.Game._PlayerScore;
            //Add Service
            GameService.Game._BattlefieldPlayer.OnPlayerAction += UpdateBoards;
            GameService.Game._BattlefieldOpponent.OnPlayerAction += UpdateBoards;
            GameService.Game.OnGameOver += GameOver;
            StateHasChanged();
        }

        public void UpdateBoards(SquareState state) 
        {
            _state = state;
			_score = GameService.Game._PlayerScore;
			StateHasChanged();
        }

        public async void GameOver(string winner)
        {
			//Show Game Winner
			await OpenDialog(winner);
			StateHasChanged();
			NavigationManager.NavigateTo("/Lobby", true);
		}

        public void HandeFieldClicked(int[] coords)
        {
            _currentXCord = coords[0];
            _currentYCord = coords[1];
        }

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

        public void Dispose()
        {
            //Dispose of Service
            GameService.Game._BattlefieldPlayer.OnPlayerAction -= UpdateBoards;
            GameService.Game._BattlefieldOpponent.OnPlayerAction -= UpdateBoards;
            GameService.Game.OnGameOver -= GameOver;
		}

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

        private void OpenAlreadyShotDialog()
        {
			DialogParameters parameters = new DialogParameters { { "ContentText", "Field can't be Shoot Again!" } };
			DialogService.Show<ShipPlacementDialog>("", parameters);
		}
	}
}
