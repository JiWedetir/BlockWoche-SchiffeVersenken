using SchiffeVersenken.Data.Controller;

namespace SchiffeVersenken.DatabaseEF.Database
{
    public class UserManagement
    {
        public static User _Player { get; private set; } = new User();
        public static User _Opponent { get; private set; } = new User();

        /// <summary>
        /// Register a new user with the given name and password
        /// </summary>
        /// <param name="name">Username</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        public static async Task<bool> RegisterUser(string name, string password)
        {
            if (await CheckUserNameExists(name))
            {
                return false;
            }
            var salt = PasswordHasher.GenerateSalt();
            User user = new User()
            {
                Name = name,
                Salt = salt,
                PasswordHash = PasswordHasher.HashPassword(password, salt)
            };  
            bool changedRows = await DatabaseAccess.SaveUserAsync(user);
            _Player = user;
            return changedRows;
        }

        /// <summary>
        /// Checks whether user name already exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true if exitsts, else false.</returns>
        private static async Task<bool> CheckUserNameExists(string name)
        {
            List<string> usernames = await DatabaseAccess.GetUserNamesAsync();
            return usernames.Contains(name);
        }

        /// <summary>
        /// Login a user with the given name and password
        /// </summary>
        /// <param name="name">Username</param>
        /// <param name="password">User password</param>
        /// <returns>true if password is valide</returns>
        public static async Task<bool> LoginUser(string name, string password)
        {
            User user = await DatabaseAccess.GetUserAsync(name);
            if (user == null)
            {
                return false;
            }
            string hash = PasswordHasher.HashPassword(password, user.Salt);
            if (hash == user.PasswordHash)
            {
                _Player = user;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns a list of all usernames in the database to choose from
        /// </summary>
        /// <returns>a list of all usernames in the database</returns>
        public static async Task<List<string>> GetUserNamesAsyync()
        {
            return await DatabaseAccess.GetUserNamesAsync();
        }

        /// <summary>
        /// Sets the opponent for the current user.
        /// </summary>
        /// <param name="userName">The name of the opponent.</param>
        public static void SetOpponent(string userName)
        {
            User user = new User();
            user.Name = userName;
            _Opponent = user;
        }

        /// <summary>
        /// Sets the default player asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SetDefaultPlayer()
        {
            _Player = await DatabaseAccess.GetUserAsync("Player");
        }

        /// <summary>
        /// Sets the computer opponent based on the specified difficulty level.
        /// </summary>
        /// <param name="opponent">The difficulty level of the computer opponent.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SetComputerOpponent(ComputerDifficulty opponent)
        {
            if (opponent == ComputerDifficulty.Dumm)
            {
                _Opponent = await DatabaseAccess.GetUserAsync("Dummer_Computer");
            }
            else if (opponent == ComputerDifficulty.Klug)
            {
                _Opponent = await DatabaseAccess.GetUserAsync("Kluger_Computer");
            }
            else if (opponent == ComputerDifficulty.Genie)
            {
                _Opponent = await DatabaseAccess.GetUserAsync("Genialer_Computer");
            }
        }
    }
}
