namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player2TurnState: PlayersTurnState
    {
        public override void HandleInput(GameLogic game, int x, int y)
        {
            game._BattlefieldPlayer.Shoot(x, y);
        }
    }
}
