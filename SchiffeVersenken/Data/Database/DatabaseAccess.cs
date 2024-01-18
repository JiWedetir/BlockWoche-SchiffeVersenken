using SQLite;

namespace SchiffeVersenken.Data.Database
{
    public class DatabaseAccess
    {
        SQLiteAsyncConnection Database;

        public DatabaseAccess()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<User>();
            await Database.CreateTableAsync<Highscore>();
        }
        public async Task<List<User>> GetUserNamesAsync()
        {
            await Init();
            List<User> users = await Database.QueryAsync<User>("SELECT Name FROM User");
            return users;
        }
        public async Task<User> GetUserAsync(int id)
        {
            await Init();
            return await Database.GetAsync<User>(id);
        }
        public async Task<User> GetUserAsync(string name)
        {
            await Init();
            return await Database.Table<User>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }
        public async Task<int> SaveUserAsync(User user)
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
        public async Task<int> DeleteUserAsync(User user)
        {
            await Init();
            return await Database.DeleteAsync(user);
        }
        public async Task<List<UserScore>> GetUserScoreAsync(string username)
        {
            await Init();
            var query = "SELECT users.name, highscores.score FROM users JOIN highscores ON users.id = highscores.user_id WHERE users.name = ? ORDER BY highscores.score DESC LIMIT 10";
            List<UserScore> userScores = await Database.QueryAsync<UserScore>(query, username);
            return userScores;
        }
        public async Task<int> UpdateScoresAsync(Highscore highscore)
        {
            await Init();
            return await Database.InsertAsync(highscore);
        }
    }
}
