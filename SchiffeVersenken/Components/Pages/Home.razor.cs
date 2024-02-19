using MudBlazor;
using SchiffeVersenken.Data.Database;

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
		private List<User> _users = new List<User>();

		private string _errorMessage = string.Empty;
		private bool _isRegisterOpen = false;

		protected override async void OnInitialized()
		{
			//Get Usernames from DB
			_users = await UserManagement.GetUserNamesAsyync();
			foreach (User user in _users)
			{
				_usernames.Add(user.Name);
			}
		}

		private async void GoToLobbyPage()
		{
			if(await UserManagement.LoginUser(_username, _password))
			{
				NavigationManager.NavigateTo("/Game", true);
			}
			else
			{
				_errorMessage = "Wrong Username or Passworr";
			}
		}

		private void ToggleRegisterPopOver()
		{
			//Open Register PopUp
			_isRegisterOpen = !_isRegisterOpen;
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
