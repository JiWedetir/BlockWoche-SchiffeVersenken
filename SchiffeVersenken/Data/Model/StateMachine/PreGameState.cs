using SchiffeVersenken.Data.View;
using System.Diagnostics;
namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class PreGameState : IBattleShipsGameState
    {
        public void AfterEnterState(GameLogic game)
        {
        }

        public void EnterState(GameLogic game)
        {
            Debug.WriteLine("Spiellogik: GameBoard wird generiert");
            game._BattlefieldPlayer = new BattlefieldPlayer(game._Size);
            game._BattlefieldOpponent = new BattlefieldOpponent(game._Size);
            game._ComputerOpponent.SetShipRandom();
        }

        public void ExitState(GameLogic game)
        {
        }

        public void HandleInput(GameLogic game, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
