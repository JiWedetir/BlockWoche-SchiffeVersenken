using SchiffeVersenken.Data.Sea;
using SchiffeVersenken.Data.Model;
using Microsoft.AspNetCore.Components;

namespace SchiffeVersenken.Components.Pages
{
    public partial class GameBattle
    {
		[CascadingParameter]
		public GameLogic Game { get; set; }
        private char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private Square[,] _ownfield = null;
        private Square[,] _enemyfield = null;
        private bool _playerTurn = true;
        private char _currentXCord;
        private int _currentYCord;

		private Dictionary<string, string> _bgUrls = new Dictionary<string, string>()
        {
            { "placement", "url(../images/backgroundbattlefieldown.png)" },
            { "own", "url(../images/ownfield.png)" },
            { "enemy", "url(../images/enemyfield.png)" }
        };

        protected override void OnInitialized()
        {
            StateHasChanged();
            _ownfield = Game._BattlefieldPlayer._Board;
            _enemyfield = Game._BattlefieldOpponent._Board;
        }

        public void UpdateBoard()
        {

        }

        public void HandeFieldClicked(int[] coords)
        {
            _currentXCord = _alphabet[coords[0]];
            _currentYCord = coords[1];
        }
    }
}
