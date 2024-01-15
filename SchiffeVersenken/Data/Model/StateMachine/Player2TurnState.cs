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
            //Battlefield gewechslet zum testen!
            bool hit = await game._BattlefieldOpponent.ShootAsync(x, y);
            if (!hit)
            {
                game._OpponentScore++;
            }
            game._ComputerOpponent._shootHistory.Add((x, y, hit, false));
            game.shoots.Add((x, y, hit));
            bool gameOver = game._BattlefieldOpponent.CheckGameOver();
            game.SelectPlayer(hit, gameOver);
        }
    }
}
