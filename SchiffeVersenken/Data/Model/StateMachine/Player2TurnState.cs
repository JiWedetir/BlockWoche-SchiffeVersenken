namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player2TurnState: PlayersTurnState
    {
        
        public override void AfterEnterState(GameLogic game)
        {
            game._Opponent.ShootAsync();
        }
        public override async Task HandleInput(GameLogic game, int x, int y)
        {
<<<<<<< HEAD
            bool hit = await game._BattlefieldPlayer.ShootAsync(x, y);
            if (!hit)
            {
                game._OpponentScore++;
            }
=======
            //Battlefield gewechslet zum testen!
            bool hit = await game._BattlefieldOpponent.ShootAsync(x, y);
>>>>>>> 1be2ca8 (Messages lesen und schreiben)
            game._ComputerOpponent._shootHistory.Add((x, y, hit, false));
            bool gameOver = game._BattlefieldPlayer.CheckGameOver();
            game.SelectPlayer(hit, gameOver);
        }
    }
}
