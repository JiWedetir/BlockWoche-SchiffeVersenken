namespace SchiffeVersenken.Data.Model.StateMachine
{
    public abstract class PlayersTurnState : IBattleShipsGameState
    {
        public void AfterEnterState(GameLogic game)
        {
            throw new NotImplementedException();
        }

        public void EnterState(GameLogic game)
        {
            throw new NotImplementedException();
        }

        public void ExitState(GameLogic game)
        {
            throw new NotImplementedException();
        }

        public virtual void HandleInput(GameLogic game, int x, int y)
        {
            
        }
    }
}
