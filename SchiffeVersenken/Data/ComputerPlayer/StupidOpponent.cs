using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Sea;
using SchiffeVersenken.Data.Model;

namespace SchiffeVersenken.Data.ComputerPlayer
{
    public abstract class StupidOpponent
    {
        protected Battlefield _battlefield;
        protected ComputerOpponent _computer;
        protected Random _random = new Random();
        protected int _x;
        protected int _y;
        protected int[] _shipsToFinde = { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
        public int _X { get { return _x; } }
        public int _Y { get { return _y; } }
        public StupidOpponent(GameLogic game)
        {
            _battlefield = game._BattlefieldPlayer;
            _computer = game._ComputerOpponent;
        }

        public bool SelectSquare()
        {
            _x = _random.Next(_battlefield._Size);
            _y = _random.Next(_battlefield._Size);
            if (_battlefield._Board[_x, _y]._State == SquareState.Hit || _battlefield._Board[_x, _y]._State == SquareState.Miss || _battlefield._Board[_x, _y]._State == SquareState.Sunk || _battlefield._Board[_x, _y]._State == SquareState.Blocked)
            {
                SelectSquare();
            }
            return true;
        }
    }
}
