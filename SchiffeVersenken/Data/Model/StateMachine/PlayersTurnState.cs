namespace SchiffeVersenken.Data.Model.StateMachine
{
    public abstract class PlayersTurnState : IBattleShipsGameState
    {
        public virtual void AfterEnterState(GameLogic game)
        {

        }

        public virtual void EnterState(GameLogic game)
        {

        }

        public virtual void ExitState(GameLogic game)
        {

        }

        public virtual async Task HandleInput(GameLogic game, int x, int y)
        {
            
        }
    }
}
