using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Model;



namespace SchiffeVersenken.Components.Pages
{
	public partial class PregameVsComputer
    {
        [CascadingParameter]
        public GameLogic Game { get; set; }
        private const int _minFieldSize = 8;
        private const int _maxFieldSize = 15;
        private string _minFieldSizeString = $"{_minFieldSize}x{_minFieldSize}";
        private string _maxFieldSizeString = $"{_maxFieldSize}x{_maxFieldSize}";

        private int _FieldSize { get; set; } = _minFieldSize;
        private ComputerDifficulty _Difficulty { get; set; } = ComputerDifficulty.Dumm;

        private void SendSettings()
        {
            //Game._Player  fieldsize
            //Game._Opponent  difficulty
            ChangePage();
        }

        private void ChangePage()
        {
            //Game._Player.SetBoardSize(_FieldSize);
            //Game._ComputerOpponent.SetDificulty(_Difficulty);
            NavigationManager.NavigateTo("/ShipPlacement", true);
        }
    }
}
