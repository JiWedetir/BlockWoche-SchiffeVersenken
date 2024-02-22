namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player2TurnState : IBattleShipsGameState
    {
        
        /// <summary>
        /// Executes the logic after entering the Player2TurnState.
        /// If the game is a local game, it sleeps for 500ms to allow the player to see where the computer shot and then triggers the opponent's shoot asynchronously.
        /// </summary>
        /// <param name="game">The game logic instance.</param>
        public void AfterEnterState(GameLogic game)
        {
            if (game._LocalGame)
            {
                // sleep for 500ms to make sure the player sees where the computer shot
                Thread.Sleep(500);
                game._Opponent.ShootAsync();
            }
        }

        /// <summary>
        /// Enters the player 2 turn state and updates the game logic to indicate that it is the opponent's turn.
        /// </summary>
        /// <param name="game">The game logic instance.</param>
        public void EnterState(GameLogic game)
        {
            game._Opponent._YourTurn = true;
        }

        /// <summary>
        /// Exits the current state and updates the game logic to indicate that it is no longer the opponent's turn.
        /// </summary>
        /// <param name="game">The game logic instance.</param>
        public void ExitState(GameLogic game)
        {
            game._Opponent._YourTurn = false;
        }

        /// <summary>
        /// Handles the input for the player's turn in the game and shoots at the opponent's battlefield.
        /// </summary>
        /// <param name="game">The game logic instance.</param>
        /// <param name="x">The x-coordinate of the target position.</param>
        /// <param name="y">The y-coordinate of the target position.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task HandleInput(GameLogic game, int x, int y)
        {
            bool hit = await game._BattlefieldPlayer.ShootAsync(x, y);
            game._ComputerOpponent._shootHistory.Add((x, y, hit, false));
            bool gameOver = game._BattlefieldPlayer.CheckGameOver();
            game.SelectPlayer(hit, gameOver);
        }
    }
}
