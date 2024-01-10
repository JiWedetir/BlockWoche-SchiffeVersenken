using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.ComputerPlayer
{
    public abstract class CleverOpponent: StupidOpponent
    {
        protected bool _cleverFieldFound;
        public CleverOpponent(Battlefield battlefield, ComputerOpponent computer) : base(battlefield, computer)
        {
        }

        public void ShootClever()
        {
            _cleverFieldFound = false;
            foreach (Square square in _battlefield._Board)
            {
                if (square._State == SquareState.Hit)
                {
                    _cleverFieldFound = CheckAdjacentSquares();
                }
                else if (square._State == SquareState.Sunk)
                {
                    MarkAdjacentSquares(_x, _y);
                    RemoveShipFromShipToFind(square._Ship._Length);
                }
            }
            if (!_cleverFieldFound)
            {
                SelectSquare();
            }
        }
        private void MarkAdjacentSquares(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (Math.Abs(i) == Math.Abs(j))
                        continue;

                    int checkX = x + i;
                    int checkY = y + j;

                    if (checkX >= 0 && checkX < _battlefield._Size && checkY >= 0 && checkY < _battlefield._Size)
                    {
                        if (_battlefield._Board[checkX, checkY]._State == SquareState.Empty)
                        {
                            _battlefield._Board[checkX, checkY]._State = SquareState.Blocked;
                        }
                    }
                }
            }
        }

        private bool CheckAdjacentSquares()
        {
            if (_y > 0 && _battlefield._Board[_x, _y - 1]._State == SquareState.Hit || _y < _battlefield._Size - 1 && _battlefield._Board[_x, _y + 1]._State == SquareState.Hit)
            {
                if (_y > 0 && _battlefield._Board[_x, _y - 1]._State == SquareState.Empty)
                {
                    _y--;
                    return true;
                }
                else if (_y < _battlefield._Size - 1 && _battlefield._Board[_x, _y + 1]._State == SquareState.Empty)
                {
                    _y++;
                    return true;
                }
            }

            if (_x > 0 && _battlefield._Board[_x - 1, _y]._State == SquareState.Hit ||
                _x < _battlefield._Size - 1 && _battlefield._Board[_x + 1, _y]._State == SquareState.Hit)
            {
                if (_x > 0 && _battlefield._Board[_x - 1, _y]._State == SquareState.Empty)
                {
                    _x--;
                    return true;
                }
                else if (_x < _battlefield._Size - 1 && _battlefield._Board[_x + 1, _y]._State == SquareState.Empty)
                {
                    _x++;
                    return true;
                }
            }

            return false;
        }

        private void RemoveShipFromShipToFind(int length)
        {
            int indexToRemove = Array.IndexOf(_shipsToFinde, length);
            if (indexToRemove != -1)
            {
                _shipsToFinde = _shipsToFinde.Where((val, idx) => idx != indexToRemove).ToArray();
            }
        }
    }
}
