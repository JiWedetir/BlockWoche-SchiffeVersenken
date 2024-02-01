namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player1TurnState : IBattleShipsGameState
    {
        public void AfterEnterState(GameLogic game)
        {
        }

        public void EnterState(GameLogic game)
        {
            game._Player._YourTurn = true;
        }
        public void ExitState(GameLogic game)
        {
            game._Player._YourTurn = false;
        }
        public async Task HandleInput(GameLogic game, int x, int y)
        {
            bool hit = await game._BattlefieldOpponent.ShootAsync(x, y);
            if (!hit)
            {
                game._PlayerScore--;
            }
            bool gameOver = game._BattlefieldOpponent.CheckGameOver();
            game.SelectPlayer(hit, gameOver);
        }
    }
}
