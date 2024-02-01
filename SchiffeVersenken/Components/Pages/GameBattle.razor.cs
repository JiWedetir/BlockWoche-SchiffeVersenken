using SchiffeVersenken.Data.Sea;
using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.View;
using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data.Model.StateMachine;

namespace SchiffeVersenken.Components.Pages
{
    public partial class GameBattle : IDisposable
    {
		[CascadingParameter] public GameLogic Game { get; set; }

		private char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private Square[,] _ownfield = null;
        private Square[,] _enemyfield = null;
        private bool _playerTurn;
        private int _currentXCord;
        private int _currentYCord;
        private SquareState _state;
        private int _score;
        private string _winner;

        protected override void OnInitialized()
        {
            _ownfield = Game._BattlefieldPlayer._Board;
            _enemyfield = Game._BattlefieldOpponent._Board;
            _playerTurn = Game._Player._YourTurn;
            _score = Game._PlayerScore;
            //Add Service
            Game._BattlefieldPlayer.OnPlayerAction += UpdateBoards;
            Game._BattlefieldOpponent.OnPlayerAction += UpdateBoards;
            Game.OnGameOver += GameOver;
            StateHasChanged();
        }

        public void UpdateBoards(SquareState state) 
        {
            _state = state;
			_score = Game._PlayerScore;
			StateHasChanged();
        }

        public void GameOver(string winner)
        {
			//Show Game Winner
			_winner = winner;
			StateHasChanged();
		}

        public void HandeFieldClicked(int[] coords)
        {
            _currentXCord = coords[0];
            _currentYCord = coords[1];
        }

        public void OnPlayerShoot()
        {
            Game._Player.Shoot(_currentXCord, _currentYCord);
        }

        public void Dispose()
        {
            //Dispose of Service
            Game._BattlefieldPlayer.OnPlayerAction -= UpdateBoards;
            Game._BattlefieldOpponent.OnPlayerAction -= UpdateBoards;
			Game.OnGameOver -= GameOver;
		}

	}
}
