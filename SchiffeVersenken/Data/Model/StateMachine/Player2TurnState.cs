using SchiffeVersenken.Data.Controller;

namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player2TurnState: PlayersTurnState
    {
        public override void AfterEnterState(GameLogic game)
        {
            game._ComputerOpponent.Shoot();
        }
        public override void HandleInput(GameLogic game, int x, int y)
        {
            bool hit = game._BattlefieldPlayer.Shoot(x, y);
            bool gameOver = game._BattlefieldPlayer.CheckGameOver();
            game.SelectPlayer(hit, gameOver);
        }
    }
}
