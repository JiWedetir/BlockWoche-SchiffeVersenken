namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player2TurnState : IBattleShipsGameState
    {
        
        public void AfterEnterState(GameLogic game)
        {
            if (game._LocalGame)
            {
                // sleep for 500ms to make sure the player sees where the computer shot
                Thread.Sleep(500);
                game._Opponent.ShootAsync();
            }
        }

        public void EnterState(GameLogic game)
        {
            game._Opponent._YourTurn = true;
        }

        public void ExitState(GameLogic game)
        {
            game._Opponent._YourTurn = false;
        }

        public async Task HandleInput(GameLogic game, int x, int y)
        {
            bool hit = await game._BattlefieldPlayer.ShootAsync(x, y);
            game._ComputerOpponent._shootHistory.Add((x, y, hit, false));
            bool gameOver = game._BattlefieldPlayer.CheckGameOver();
            game.SelectPlayer(hit, gameOver);
        }
    }
}
