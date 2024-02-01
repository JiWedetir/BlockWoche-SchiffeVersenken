using SQLite;

namespace SchiffeVersenken.Data.Database
{
    internal class DatabaseAccess
    {
        SQLiteAsyncConnection Database;

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<User>();
            await Database.CreateTableAsync<Highscore>();
        }
        internal async Task<List<User>> GetUserNamesAsync()
        {
            await Init();
            List<User> users = await Database.QueryAsync<User>("SELECT Name FROM User");
            return users;
        }
        internal async Task<User> GetUserAsync(int id)
        {
            await Init();
            return await Database.GetAsync<User>(id);
        }
        internal async Task<User> GetUserAsync(string name)
        {
            await Init();
            return await Database.Table<User>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }
        internal async Task<int> SaveUserAsync(User user)
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
        internal async Task<int> DeleteUserAsync(User user)
        {
            await Init();
            return await Database.DeleteAsync(user);
        }
        internal async Task<List<UserScore>> GetUserScoreAsync(string username)
        {
            await Init();
            var query = "SELECT users.name, highscores.score FROM users JOIN highscores ON users.id = highscores.user_id WHERE users.name = ? ORDER BY highscores.score DESC LIMIT 10";
            List<UserScore> userScores = await Database.QueryAsync<UserScore>(query, username);
            return userScores;
        }
        internal async Task<int> UpdateScoresAsync(Highscore highscore)
        {
            await Init();
            return await Database.InsertAsync(highscore);
        }
    }
}
