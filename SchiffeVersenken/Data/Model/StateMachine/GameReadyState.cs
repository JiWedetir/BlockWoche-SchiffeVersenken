using System.Diagnostics;

namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class GameReadyState : IBattleShipsGameState
    {
        public void AfterEnterState(GameLogic game)
        {
            throw new NotImplementedException();
        }

        public void EnterState(GameLogic game)
        {
            Debug.WriteLine("Spiellogik: Spiel ist bereit");
            
        }

        public void ExitState(GameLogic game)
        {
            throw new NotImplementedException();
        }

        public void HandleInput(GameLogic game, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
