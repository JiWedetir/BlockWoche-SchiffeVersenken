namespace SchiffeVersenken.Data.Database
{
    public class HighScores
    {
        /// <summary>
        /// Gets the 10 best scores for the given username
        /// </summary>
        /// <param name="username">string username</param>
        /// <returns>List<UserScore> with the 10 best scores</UserScore></returns>
        public async static Task<List<UserScore>> GetHighScores(string username)
        {
            DatabaseAccess db = new DatabaseAccess();
            return await db.GetUserScoreAsync(username);
        }

        /// <summary>
        /// Saves the given score for the given username
        /// </summary>
        /// <param name="username">string username</param>
        /// <param name="opponent">string opponent</param>
        /// <param name="score">int gamescore</param>
        /// <returns></returns>
        public async static Task<bool> SaveHighScore(string winner, int score)
        {
            DatabaseAccess db = new DatabaseAccess();
            if (UserManagement._Player == null)
            {
                return false;
            }
            bool won = UserManagement._Player.Name == winner;
            Highscore highscore = new Highscore();
            highscore.User_Id = UserManagement._Player.Id;
            highscore.Opponent = UserManagement._Opponent.Name;
            highscore.Score = score;
            highscore.Won = won;
            int changedRows = await db.UpdateScoresAsync(highscore);
            return changedRows > 0;
        }
    }
}
