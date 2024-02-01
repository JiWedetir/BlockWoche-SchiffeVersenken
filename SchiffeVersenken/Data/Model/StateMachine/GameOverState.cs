using SchiffeVersenken.Data.Database;

namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class GameOverState : IBattleShipsGameState
    {
		public void AfterEnterState(GameLogic game)
        {
        }

        public void EnterState(GameLogic game)
        {
            HighScores.SaveHighScore(game._Winner, game._Looser, game._PlayerScore);
		}

        public void ExitState(GameLogic game)
        {
        }

        public Task HandleInput(GameLogic game, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
