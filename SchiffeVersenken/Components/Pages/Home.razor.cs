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
		private bool isShow = false;
		private InputType PasswordInput = InputType.Password;
		private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
		private bool _loginLoading = false;

		private List<string> _usernames = new List<string>();
		private List<User> _users = new List<User>();

		private string _errorMessage = string.Empty;
		private bool _isRegisterOpen = false;

		//For Registration
		private string _registerUsername = string.Empty;
		private string _registerPasswordFirst = string.Empty;
		private string _registerPasswordSecond = string.Empty;
		private string _registationErrorMessage = string.Empty;
		private bool _newUserRegistered = false;
		private bool _registrationLoading = false;

		protected override async void OnInitialized()
		{
			//Get Usernames from DB
			await GetUsernameFromDB();
		}

		private async Task GetUsernameFromDB()
		{
			_users = await UserManagement.GetUserNamesAsyync();
			foreach (User user in _users)
			{
				_usernames.Add(user.Name);
			}
		}

		private async void GoToLobbyPage()
		{
			_loginLoading = true;
			StateHasChanged();
			if (await UserManagement.LoginUser(_username, _password))
			{
				_loginLoading = false;
				NavigationManager.NavigateTo("/Lobby", true);
			}
			else
			{
				_errorMessage = "Wrong Username or Passworr";
			}
			_loginLoading = false;
		}

		private async void ToggleRegisterPopOver()
		{
			//Open Register PopUp
			_isRegisterOpen = !_isRegisterOpen;
			if (_newUserRegistered)
			{
				await GetUsernameFromDB();
			}
		}

		private async Task<IEnumerable<string>> SearchUsers(string value)
		{
			// if text is null or empty, show complete list
			if (string.IsNullOrEmpty(value))
				return _usernames;
			return _usernames.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
		}
		private IEnumerable<string> MaxCharacters(string ch)
		{
			if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
				yield return "Max 25 characters";
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

		private async void CheckRegistration()
		{
			if (_registerPasswordFirst.Equals(_registerPasswordSecond))
			{
				_registrationLoading = true;
				StateHasChanged();
				if (await UserManagement.RegisterUser(_registerUsername, _registerPasswordFirst))
				{
					_newUserRegistered = true;
					ToggleRegisterPopOver();
				}
				else
				{
					_registationErrorMessage = "Username already exists!";
				}
				_registrationLoading = false;
			}
			else
			{
				_registationErrorMessage = "Passwords do not match!";
			}
		}

	}
}
