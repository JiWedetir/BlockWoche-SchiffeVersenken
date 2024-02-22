using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data;
using SchiffeVersenken.Data.Controller;


namespace SchiffeVersenken.Components.Pages
{
    public partial class PregameVsComputer
    {
        // Gets the game logic service instance, cascaded from a parent component
        [CascadingParameter]
        public GameLogicService GameService { get; set; }

        // Path to the background image
        private string _bgUrl = "url('../images/backgroundsettings.png')";

        // UI-Display values for fieldsize choosing
        private const int minFieldSize = 9;
        private const int maxFieldSize = 15;
        private string[] labels =  new string[] { $"{minFieldSize}x{minFieldSize}", "", "", "", "", "", $"{ maxFieldSize }x{ maxFieldSize }" };

        // Selected field size for the game
        private int _FieldSize { get; set; } = minFieldSize;

        // Selected computer difficulty level
        private ComputerDifficulty _Difficulty { get; set; } = ComputerDifficulty.Dumm;


        /// <summary>
        /// Initializes the component and starts a new game instance.
        /// </summary>
		protected override void OnInitialized()
		{
            GameService.CreateNewGame();
		}

        /// <summary>
        /// Sends the selected game settings and navigates to the ship placement page
        /// </summary>
		private void SendSettings()
        {
            GameService.Game.StartPlacingShips(_FieldSize, _Difficulty);
            NavigationManager.NavigateTo("/ShipPlacement", true);
        }
    }
}
