﻿namespace SchiffeVersenken.Data.Model.StateMachine
{
    public interface IBattleShipsGameState
    {
        void ExitState(GameLogic game);
        void EnterState(GameLogic game);
        void AfterEnterState(GameLogic game);
        void HandleInput(GameLogic game, int x, int y);

    }
}
