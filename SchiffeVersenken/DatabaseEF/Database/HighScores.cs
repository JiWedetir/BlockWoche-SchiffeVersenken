namespace SchiffeVersenken.DatabaseEF.Database
{
    public class HighScores
    {

        /// <summary>
        /// Gets the 10 best scores for the given username
        /// </summary>
        /// <param name="username">string username</param>
        /// <returns>List<UserScore> with the 10 best scores</UserScore></returns>
        public async static Task<List<UserScoreView>> GetHighScores(string username)
        {
            return await DatabaseAccess.GetUserScoreAsync(username);
        }

        /// <summary>
        /// Saves the given score for the given username
        /// </summary>
        /// <param name="winner">string winner</param>
        /// <param name="score">int gamescore</param>
        /// <returns></returns>
        public async static Task<bool> SaveHighScore(string winner, int score)
        {
            if (UserManagement._Player == null)
            {
                return false;
            }
            Highscore highscore = new Highscore()
            {
                User_Id = UserManagement._Player.Id,
                Opponent = UserManagement._Opponent.Name,
                Score = score,
                Won = UserManagement._Player.Name == winner
            };
            bool changedRows = await DatabaseAccess.UpdateScoresAsync(highscore);
            return changedRows;
        }
    }
}
