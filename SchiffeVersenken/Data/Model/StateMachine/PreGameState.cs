using SchiffeVersenken.Data.View;
using System.Diagnostics;
namespace SchiffeVersenken.Data.Model.StateMachine
{
    public class PreGameState : IBattleShipsGameState
    {
        private int _size;
        public void AfterEnterState(GameLogic game)
        {
            Debug.WriteLine("Spiellogik: GameBoard wird generiert");
            game._BattelfieldPlayer = new BattlefieldPlayer(_size);
            game._BattelfieldOpponent = new BattlefieldOpponent(_size);
            game._BattelfieldOpponent.SetShipRandom();
        }

        public void EnterState(GameLogic game)
        {
            //den spieler fragen wie groß das Spielfeld sein soll
            throw new NotImplementedException();
        }

        public void ExitState(GameLogic game)
        {
            throw new NotImplementedException();
        }

        public void HandleInput(GameLogic game, int x, int y)
        {
            throw new NotImplementedException();
        }
        public void SetSize(int size)
        {
            _size = size;
        }
    }
}
