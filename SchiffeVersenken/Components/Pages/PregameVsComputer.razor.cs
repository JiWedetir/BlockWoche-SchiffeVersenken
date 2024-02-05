using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Model;



namespace SchiffeVersenken.Components.Pages
{
	public partial class PregameVsComputer
    {
        [CascadingParameter]
        public GameLogic Game { get; set; }
        private const int _minFieldSize = 9;
        private const int _maxFieldSize = 15;
        private string _minFieldSizeString = $"{_minFieldSize}x{_minFieldSize}";
        private string _maxFieldSizeString = $"{_maxFieldSize}x{_maxFieldSize}";

        private string _bgUrl = "url('../images/backgroundsettings.png')";

        string[] labels;

		private int _FieldSize { get; set; } = _minFieldSize;
        private ComputerDifficulty _Difficulty { get; set; } = ComputerDifficulty.Dumm;

		protected override void OnInitialized()
		{
			base.OnInitialized();
			labels = new string[] { _minFieldSizeString, "", "", "", "", "", _maxFieldSizeString };

		}

		private void SendSettings()
        {
            Game.StartPlacingShips(_FieldSize, _Difficulty);
            ChangePage();
        }

        private void ChangePage()
        {
            NavigationManager.NavigateTo("/ShipPlacement", true);
        }
    }
}
