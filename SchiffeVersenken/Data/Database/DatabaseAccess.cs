﻿using SQLite;
using System.Diagnostics;

namespace SchiffeVersenken.Data.Database
{
    internal class DatabaseAccess
    {
        SQLiteAsyncConnection Database;

        /// <summary>
        /// Initializes the database connection and creates necessary tables if they don't exist.
        /// </summary>
        async Task Init()
        {
            try
            {
                if (Database is not null)
                    return;

                Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                var result = await Database.CreateTableAsync<User>();
                await Database.CreateTableAsync<Highscore>();
                if (result == CreateTableResult.Created)
                {
                    await CreateDefaultUsers();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Creates the default users if they don't exist.
        /// </summary>
        internal async Task CreateDefaultUsers()
        {
            try
            {
                User player = new User() { Name = "Player", PasswordHash="1234", Salt="1234" };
                User dumm = new User() { Name = "Dummer_Computer", PasswordHash = "1234", Salt = "1234" };
                User klug = new User() { Name = "Kluger_Computer", PasswordHash = "1234", Salt = "1234" };
                User genial = new User() { Name = "Genialer_Computer", PasswordHash = "1234", Salt = "1234" };
                await Database.InsertAllAsync(new List<User>() { player, dumm, klug, genial });
                List<User> users = await Database.Table<User>().ToListAsync();
                foreach (User user in users)
                {
                    Debug.WriteLine(user.Name);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of user names from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of user names.</returns>
        internal async Task<List<User>> GetUserNamesAsync()
        {
            try
            {
                await Init();
                List<User> users = await Database.QueryAsync<User>("SELECT Name FROM User WHERE Name NOT IN ('Player', 'Dummer_Computer', 'Kluger_Computer', 'Genialer_Computer')");
                return users;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<User>();
            }
        }

        /// <summary>
        /// Retrieves a user from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user object if found, or null if an error occurs.</returns>
        internal async Task<User> GetUserAsync(int id)
        {
            try
            {
                await Init();
                return await Database.GetAsync<User>(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new User();
            }
        }

        /// <summary>
        /// Retrieves a user from the database based on their name.
        /// </summary>
        /// <param name="name">The name of the user to retrieve.</param>
        /// <returns>The user object if found, otherwise null.</returns>
        internal async Task<User> GetUserAsync(string name)
        {
            try
            {
                await Init();
                return await Database.Table<User>().Where(i => i.Name == name).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new User();
            }
        }

        /// <summary>
        /// Saves a user asynchronously.
        /// </summary>
        /// <param name="user">The user to save.</param>
        /// <returns>The number of rows affected in the database.</returns>
        internal async Task<int> SaveUserAsync(User user)
        {
            try
            {
                await Init();
                if (user.Id != 0)
                {
                    return await Database.UpdateAsync(user);
                }
                else
                {
                    return await Database.InsertAsync(user);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Deletes a user from the database asynchronously.
        /// </summary>
        /// <param name="user">The user to be deleted.</param>
        /// <returns>The number of rows affected in the database.</returns>
        internal async Task<int> DeleteUserAsync(User user)
        {
            try
            {
                await Init();
                return await Database.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Retrieves the user scores for a given username.
        /// </summary>
        /// <param name="username">The username to retrieve scores for.</param>
        /// <returns>A list of UserScore objects representing the user scores.</returns>
        internal async Task<List<UserScore>> GetUserScoreAsync(string username)
        {
            try
            {
                await Init();
                var query = "SELECT User.Name, Highscore.Score, Highscore.Opponent, Highscore.Won FROM User JOIN Highscore ON User.Id = Highscore.User_Id WHERE User.Name = ? ORDER BY Highscore.Score DESC LIMIT 10";
                List<UserScore> userScores = await Database.QueryAsync<UserScore>(query, username);
                return userScores;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<UserScore>();
            }
        }

        /// <summary>
        /// Updates the scores in the database with the provided highscore.
        /// </summary>
        /// <param name="highscore">The highscore to be updated.</param>
        /// <returns>The number of rows affected in the database.</returns>
        internal async Task<int> UpdateScoresAsync(Highscore highscore)
        {
            try
            {
                await Init();
                return await Database.InsertAsync(highscore);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
