using System.Diagnostics;

namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class GameReadyState : IBattleShipsGameState
    {
        public void AfterEnterState(GameLogic game)
        {

        }

        public void EnterState(GameLogic game)
        {
            Debug.WriteLine("Spiellogik: Spiel ist bereit");
            game._Player1TurnState = new Player1TurnState();
            game._Player2TurnState = new Player2TurnState();
        }

        public void ExitState(GameLogic game)
        {
        }

        public Task HandleInput(GameLogic game, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
