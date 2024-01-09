namespace SchiffeVersenken.Data.Model.StateMachine
{
    public interface IBattleShipsGameState
    {
        void ExitState(GameLogic game);
        void EnterState(GameLogic game);

    }
}
