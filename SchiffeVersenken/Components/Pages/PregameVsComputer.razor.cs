using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchiffeVersenken.Components.Shared;
using SchiffeVersenken.Data;
using SchiffeVersenken.Data.Controller;
using static Microsoft.Maui.ApplicationModel.Permissions;


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

		/// <summary>
		/// Opens a Info PopUp with the given text
		/// </summary>
		private void OpenDialogInfo()
		{
			string text =   $"<b>PregameVsComputer Instructions:</b>\n" +
							$"&#8226; By dragging the slider you can choose the field size you would like. Keep in mind, the bigger the field, the longer the game will take\n" +
							$"&#8226; By clicking on the below select field, you can choose the computer diffulty you want to play against.\n" +
							$"&emsp;&#8226; 'Dumb' Opponent: Shoots randomly without paying attention to anything.\n" +
							$"&emsp;&#8226; 'Klug' Opponent: Shoots randomly, but if it hits a ship, it shoots around the hit square to sink the ship.\n" +
							$"&emsp;&#8226; 'Genie' Opponent: Like the Smart Opponent, but calculates the most likely ship position and then shoots there.\n" +
							$"&#8226; When you want to continue to placing your ships, simply click the 'Start Placings Ships' Button.";
			DialogParameters parameters = new DialogParameters { { "ContentText", text } };
			DialogService.Show<InfoDialog>("", parameters);
		}
	}
}
