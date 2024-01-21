namespace SchiffeVersenken.Components.Pages
{
    public partial class Game
    {
        private char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private int[,] _ownfield = new int[10, 10];
        private int[,] _enemyfield = new int[10, 10];
        private bool _playerTurn = true;

        private Dictionary<string, string> _bgUrls = new Dictionary<string, string>()
        {
            { "placement", "url(../images/backgroundbattlefieldown.png)" },
            { "own", "url(../images/ownfield.png)" },
            { "enemy", "url(../images/enemyfield.png)" }
        };

        private void OnInitialized()
        {
            StateHasChanged();
        }
    }
}
