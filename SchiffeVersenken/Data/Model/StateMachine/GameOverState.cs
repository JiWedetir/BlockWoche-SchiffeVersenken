namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class GameOverState : IBattleShipsGameState
    {
        public void AfterEnterState(GameLogic game)
        {
        }

        public void EnterState(GameLogic game)
        {
        }

        public void ExitState(GameLogic game)
        {
        }

        public void HandleInput(GameLogic game, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
