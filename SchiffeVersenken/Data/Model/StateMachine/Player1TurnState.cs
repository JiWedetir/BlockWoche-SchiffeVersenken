namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player1TurnState : IBattleShipsGameState
    {
        public void AfterEnterState(GameLogic game)
        {
        }

        /// <summary>
        /// Enters the player 1 turn state and updates the game logic to indicate that it is the player's turn.
        /// </summary>
        /// <param name="game">The game logic instance.</param>
        public void EnterState(GameLogic game)
        {
            game._Player._YourTurn = true;
        }

        /// <summary>
        /// Exits the current state and updates the game logic to indicate that it is no longer the player's turn.
        /// </summary>
        /// <param name="game">The game logic instance.</param>
        public void ExitState(GameLogic game)
        {
            game._Player._YourTurn = false;
        }

        /// <summary>
        /// Handles the input for the player's turn in the game and shoots at the opponent's battlefield.
        /// </summary>
        /// <param name="game">The game logic instance.</param>
        /// <param name="x">The x-coordinate of the target position.</param>
        /// <param name="y">The y-coordinate of the target position.</param>
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
