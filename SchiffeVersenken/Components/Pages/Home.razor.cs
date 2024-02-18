using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken.Components.Pages
{
	public partial class Home
	{
		private string _bgUrl = "url('../images/backgroundlogin.png')";
		private bool _loginIsValid = false;

		private string _username = string.Empty;
		private string _password = string.Empty;
		bool isShow;
		InputType PasswordInput = InputType.Password;
		string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

		private List<string> _usernames = new List<string>();

		protected override void OnInitialized()
		{
			//Get Usernames from DB
		}

		private void GoToLobbyPage()
		{
			//Only if login is valid
			NavigationManager.NavigateTo("/Game", true);
		}

		private void OpenRegisterPopUp()
		{
			//Open Register PopUp

			StateHasChanged();
		}

		private async Task<IEnumerable<string>> SearchUsers(string value)
		{
			// if text is null or empty, show complete list
			if (string.IsNullOrEmpty(value))
				return _usernames;
			return _usernames.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
		}

		void ButtonSeeClicked()
		{
			if (isShow)
			{
				isShow = false;
				PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
				PasswordInput = InputType.Password;
			}
			else
			{
				isShow = true;
				PasswordInputIcon = Icons.Material.Filled.Visibility;
				PasswordInput = InputType.Text;
			}
		}

	}
}
