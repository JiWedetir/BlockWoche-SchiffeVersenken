using Microsoft.AspNetCore.Components;
using SchiffeVersenken.DatabaseEF.Database;

namespace SchiffeVersenken.Components.Pages
{
    public partial class Lobby
    {
        // Path to the background image
        private string bgUrl = "url('../images/backgroundlobby.png')";

		// Holds the Headers for UI Table Head
		private List<string> tableHeadings = new List<string>() { "Ranking", "Name", "Score", "Opponent", "Result"};
		// To Display Empty Table Message in UI
		private bool emptyHighScoreTable = true;

		// Holds user data
		private List<string[]> _highscores = new List<string[]>();
		private string _username = string.Empty;


        /// <summary>
        /// Initializes the Lobby page by fetching high scores related to the current player.
        /// </summary>
        protected override async void OnInitialized()
		{
			_username = UserManagement._Player.Name;
			List<UserScoreView>highscores = await HighScores.GetHighScores(_username);

            int ranking = 1;
			if (highscores != null && highscores.Count() > 0)
			{
                foreach (UserScoreView score in highscores)
				{
                    emptyHighScoreTable = false;
                    string[] scorearray = { ranking++.ToString(), score.Name, score.Score.ToString(), score.Opponent, score.Won ? "Won" : "Lost" };
					_highscores.Add(scorearray);
				}
			}
			StateHasChanged();
		}


        /// <summary>
        /// Navigates to the pregame setup page for playing against the computer.
        /// </summary>
        private void StartVsComputer()
		{
			NavigationManager.NavigateTo("/PregameVsComputer", true);
		}


        /// <summary>
        /// Placeholder for starting a game against another player. Functionality coming soon.
        /// </summary>
        private void StartVsPlayer()
		{
			//Coming soon
		}
	}
}
