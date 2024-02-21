using SchiffeVersenken.Data.Controller;

namespace SchiffeVersenken.Data.Database
{
    public class UserManagement
    {
        public static User _Player { get; private set; }
        public static User _Opponent { get; private set; }

        /// <summary>
        /// Register a new user with the given name and password
        /// </summary>
        /// <param name="name">Username</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        public static async Task<bool> RegisterUser(string name, string password)
        {
            DatabaseAccess db = new DatabaseAccess();
            User user = new User();
            user.Name = name;
            user.Salt = PasswordHasher.GenerateSalt();
            user.PasswordHash = PasswordHasher.HashPassword(password, user.Salt);
            int changedRows = await db.SaveUserAsync(user);
            _Player = user;
            return changedRows > 0;
        }

        /// <summary>
        /// Login a user with the given name and password
        /// </summary>
        /// <param name="name">Username</param>
        /// <param name="password">User password</param>
        /// <returns>true if password is valide</returns>
        public static async Task<bool> LoginUser(string name, string password)
        {
            DatabaseAccess db = new DatabaseAccess();
            User user = await db.GetUserAsync(name);
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
        public static async Task<List<User>> GetUserNamesAsyync()
        {
            DatabaseAccess db = new DatabaseAccess();
            return await db.GetUserNamesAsync();
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
            DatabaseAccess db = new DatabaseAccess();
            _Player = await db.GetUserAsync("Player");
        }

        /// <summary>
        /// Sets the computer opponent based on the specified difficulty level.
        /// </summary>
        /// <param name="opponent">The difficulty level of the computer opponent.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SetComputerOpponent(ComputerDifficulty opponent)
        {
            if(opponent == ComputerDifficulty.Dumm)
            {
                _Opponent = await new DatabaseAccess().GetUserAsync("Dummer_Computer");
            }
            else if (opponent == ComputerDifficulty.Klug)
            {
                _Opponent = await new DatabaseAccess().GetUserAsync("Kluger_Computer");
            }
            else if (opponent == ComputerDifficulty.Genie)
            {
                _Opponent = await new DatabaseAccess().GetUserAsync("Genialer_Computer");
            }
        }
    }
}
