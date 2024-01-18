using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken.Data.Database
{
    public class UserManagement
    {
        public User User { get; private set; }
        private DatabaseAccess db = new DatabaseAccess();

        private UserManagement()
        {

        }

        /// <summary>
        /// Register a new user with the given name and password
        /// </summary>
        /// <param name="name">Username</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        public async Task<bool> RegisterUser(string name, string password)
        {
            User user = new User();
            user.Name = name;
            user.Salt = PasswordHasher.GenerateSalt();
            user.PasswordHash = PasswordHasher.HashPassword(password, user.Salt);
            int changedRows = await db.SaveUserAsync(user);
            User = user;
            return changedRows > 0;
        }

        /// <summary>
        /// Login a user with the given name and password
        /// </summary>
        /// <param name="name">Username</param>
        /// <param name="password">User password</param>
        /// <returns>true if password is valide</returns>
        public async Task<bool> LoginUser(string name, string password)
        {
            User user = await db.GetUserAsync(name);
            if (user == null)
            {
                return false;
            }
            string hash = PasswordHasher.HashPassword(password, user.Salt);
            if (hash == user.PasswordHash)
            {
                User = user;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns a list of all usernames in the database to choose from
        /// </summary>
        /// <returns>a list of all usernames in the database</returns>
        public async Task<List<User>> GetUserNamesAsyync()
        {
            return await db.GetUserNamesAsync();
        }
    }
}
