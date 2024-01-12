﻿namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player1TurnState: PlayersTurnState
    {
        public override void EnterState(GameLogic game)
        {
            game._Player._YourTurn = true;
        }
        public override void ExitState(GameLogic game)
        {
            game._Player._YourTurn = false;
        }
        public override void HandleInput(GameLogic game, int x, int y)
        {
            bool hit = game._BattlefieldOpponent.Shoot(x, y);
            bool gameOver = game._BattlefieldOpponent.CheckGameOver();
            game.SelectPlayer(hit, gameOver);
        }
    }
}
