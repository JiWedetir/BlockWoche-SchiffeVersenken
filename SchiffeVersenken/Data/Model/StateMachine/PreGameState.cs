using SchiffeVersenken.Data.ComputerPlayer;
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
            game._BattlefieldPlayer = new BattlefieldPlayer(game);
            game._BattlefieldOpponent = new BattlefieldOpponent(game);
            game._ComputerOpponent.SetShipRandomAsync();
            game._Opponent = new IngeniousOpponent(game);
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
