using Microsoft.AspNetCore.Components;


namespace SchiffeVersenken.Components.Pages
{
	public partial class PregameVsComputer
    {
        private const int _minFieldSize = 8;
        private const int _maxFieldSize = 15;
        private string _minFieldSizeString = $"{_minFieldSize}x{_minFieldSize}";
        private string _maxFieldSizeString = $"{_maxFieldSize}x{_maxFieldSize}";

        private int _FieldSize { get; set; } = _minFieldSize;
        private string _Difficulty { get; set; } = "Dumm";

        private void SendSettings()
        {
            //Game._Player  fieldsize
            //Game._Opponent  difficulty
            ChangePage();
        }

        private void ChangePage()
        {
			NavigationManager.NavigateTo("/ShipPlacement", true);
        }
    }
}
