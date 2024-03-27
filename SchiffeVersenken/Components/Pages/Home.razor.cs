using MudBlazor;
using SchiffeVersenken.DatabaseEF.Database;

namespace SchiffeVersenken.Components.Pages
{
    public partial class Home
	{
		// Path to the background image
		private string bgUrl = "url('../images/backgroundlogin.png')";

		// Used for toggling the visibility of the password input field
		private InputType passwordInput = InputType.Password;
		private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;
		private bool passwordIsShow = false;

		// Controls the display of loaders and forms on the UI
		private bool loginLoading = false;
		private bool isRegisterOpen = false;
		private bool registrationLoading = false;

		// Holds the list of users and usernames for login functionality
		private List<string> _usernames = new List<string>();
		private string _username = string.Empty;
		private string _password = string.Empty;

		// Holds the registration input data
		private string _registerUsername = string.Empty;
		private string _registerPasswordFirst = string.Empty;
		private string _registerPasswordSecond = string.Empty;
		private bool _newUserRegistered = false;

		// Stores error messages for UI display
		private string _errorMessageLogin = string.Empty;
		private string _errorMessageRegistration = string.Empty;


		/// <summary>
		/// Initializes component state and fetches usernames from the database
		/// </summary>
		protected override async void OnInitialized()
		{
			await GetUsernameFromDB();
		}

		/// <summary>
		/// Fetches usernames from the database and updates the UI accordingly.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation.</returns>
		private async Task GetUsernameFromDB()
		{
			_usernames = await UserManagement.GetUserNamesAsyync();

			StateHasChanged();
		}

		/// <summary>
		/// Validates and processes the registration of a new user.
		/// </summary>
		private async void CheckRegistration()
		{
			if (_registerPasswordFirst.Equals(_registerPasswordSecond))
			{
				registrationLoading = true;
				StateHasChanged();
				if (await UserManagement.RegisterUser(_registerUsername, _registerPasswordFirst))
				{
					_newUserRegistered = true;
					registrationLoading = false;
					ToggleRegisterPopOver();
				}
				else
				{
					_errorMessageRegistration = "Username already exists!";
				}
				registrationLoading = false;
			}
			else
			{
				_errorMessageRegistration = "Passwords do not match!";
			}
			StateHasChanged();
		}

		/// <summary>
		/// Validates the user's credentials and navigates to the lobby page upon successful login.
		/// </summary>
		private async void GoToLobbyPage()
		{

			loginLoading = true;
			StateHasChanged();
			if (await UserManagement.LoginUser(_username, _password))
			{
				loginLoading = false;
				NavigationManager.NavigateTo("/Lobby", true);
			}
			else
			{
				_errorMessageLogin = "Wrong Username or Passworr";
			}
			loginLoading = false;
			StateHasChanged();
		}

		/// <summary>
		/// Toggles the visibility of the registration popover.
		/// </summary>
		private async void ToggleRegisterPopOver()
		{
			isRegisterOpen = !isRegisterOpen;
			if (_newUserRegistered)
			{
				await GetUsernameFromDB();
			}
		}

		/// <summary>
		/// Toggles the visibility of the password input field.
		/// </summary>
		private void TogglePasswordVisibility()
		{
			if (passwordIsShow)
			{
				passwordIsShow = false;
				passwordInputIcon = Icons.Material.Filled.VisibilityOff;
				passwordInput = InputType.Password;
			}
			else
			{
				passwordIsShow = true;
				passwordInputIcon = Icons.Material.Filled.Visibility;
				passwordInput = InputType.Text;
			}
		}

		/// <summary>
		/// Searches for users based on a given value.
		/// </summary>
		/// <param name="value">The value to search for.</param>
		/// <returns>A filtered list of usernames.</returns>
		private async Task<IEnumerable<string>> SearchUsers(string value)
		{
			if (string.IsNullOrEmpty(value))
				return _usernames;
			return _usernames.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
		}

		/// <summary>
		/// Validates the maximum character length of a given string.
		/// </summary>
		/// <param name="ch">The string to validate.</param>
		/// <returns>An enumeration of validation messages.</returns>
		private IEnumerable<string> MaxCharacters(string ch)
		{
			if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
				yield return "Max 25 characters";
		}
	}
}
