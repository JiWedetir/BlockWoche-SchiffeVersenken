namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player2TurnState: PlayersTurnState
    {
        
        public override void AfterEnterState(GameLogic game)
        {
            game._ComputerOpponent.ShootAsync();
        }
        public override async Task HandleInput(GameLogic game, int x, int y)
        {
            bool hit = await game._BattlefieldPlayer.ShootAsync(x, y);
            if (!hit)
            {
                game._OpponentScore++;
            }
            game._ComputerOpponent._shootHistory.Add((x, y, hit, false));
            bool gameOver = game._BattlefieldPlayer.CheckGameOver();
            game.SelectPlayer(hit, gameOver);
        }
    }
}
