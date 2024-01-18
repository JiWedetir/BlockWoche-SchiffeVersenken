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
        public async static Task<bool> SaveHighScore(string username, string opponent, int score)
        {
            DatabaseAccess db = new DatabaseAccess();
            User user = await db.GetUserAsync(username);
            if (user == null)
            {
                return false;
            }
            Highscore highscore = new Highscore();
            highscore.User_Id = user.Id;
            highscore.Opponent = opponent;
            highscore.Score = score;
            int changedRows = await db.UpdateScoresAsync(highscore);
            return changedRows > 0;
        }
    }
}
