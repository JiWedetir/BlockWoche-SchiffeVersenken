﻿namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class Player2TurnState : IBattleShipsGameState
    {
        
        public void AfterEnterState(GameLogic game)
        {
            game._Opponent.ShootAsync();
        }

        public void EnterState(GameLogic game)
        {
        }

        public void ExitState(GameLogic game)
        {
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
