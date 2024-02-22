using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data.Database;

namespace SchiffeVersenken.Components.Pages
{
    public partial class Lobby
    {
		private string _bgUrl = "url('../images/backgroundlobby.png')";
		private string _username = string.Empty;
		private List<string[]> _highscores = new List<string[]>();
		private List<string> _tableHeadings = new List<string>() { "Ranking", "Name", "Score", "Opponent", "Result"};
		private int _ranking = 0;

		protected override async void OnInitialized()
		{
			_username = UserManagement._Player.Name;
			List<UserScore>highscores = await HighScores.GetHighScores(_username);
			if (highscores != null)
			{
				foreach (UserScore score in highscores)
				{
					string[] scorearray = { _ranking++.ToString(), score.Name, score.Score.ToString(), score.Opponent, score.Won ? "Won" : "Lost" };
					_highscores.Add(scorearray);
				}
			}
			StateHasChanged();
		}

		private void StartVsComputer()
		{
			NavigationManager.NavigateTo("/PregameVsComputer", true);
		}

		private void StartVsPlayer()
		{
			//NavigationManager.NavigateTo("/PregameVsComputer", true);
		}
	}
}
