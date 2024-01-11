using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Sea;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace SchiffeVersenken.Data.ComputerPlayer
{
    public abstract class CleverOpponent: StupidOpponent
    {
        protected bool _cleverFieldFound;
        protected int _size;
        public CleverOpponent(Battlefield battlefield, ComputerOpponent computer) : base(battlefield, computer)
        {
        }

        public void ShootClever()
        {
            _cleverFieldFound = false;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_battlefield._Board[i, j]._State == SquareState.Hit)
                    {
                        _cleverFieldFound = CheckAdjacentSquares(i, j);
                    }
                    else if (_battlefield._Board[i, j]._State == SquareState.Sunk)
                    {
                        MarkAdjacentSquares(i, j);
                        RemoveShipFromShipToFind(_battlefield._Board[i, j]._Ship._Length);
                    }
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

        private bool CheckAdjacentSquares(int x, int y)
        {
            if (CheckVerticalNeighbors(x, y))
            {
                return true;
            }

            if (CheckHorizontalNeighbors(x, y))
            {
                return true;
            }
            else
            {
                if(y > 0 && _battlefield._Board[x, y - 1]._State == SquareState.Empty)
                {
                    _y = y - 1;
                    _x = x;
                    return true;
                }
                else if (y < _battlefield._Size - 1 && _battlefield._Board[x, y + 1]._State == SquareState.Empty)
                {
                    _y = y + 1;
                    _x = x;
                    return true;
                }
                else if (x > 0 && _battlefield._Board[x - 1, y]._State == SquareState.Empty)
                {
                    _x = x - 1;
                    _y = y;
                    return true;
                }
                else if (x < _battlefield._Size - 1 && _battlefield._Board[x + 1, y]._State == SquareState.Empty)
                {
                    _x = x + 1;
                    _y = y;
                    return true;
                }
            }
            return false;
        }

        private bool CheckVerticalNeighbors(int x, int y)
        {
            if (y > 0 && _battlefield._Board[x, y - 1]._State == SquareState.Hit)
            {
                if (SearchInRow(x, y))
                {
                    return true;
                }
            }
            if (y < _battlefield._Size - 1 && _battlefield._Board[x, y + 1]._State == SquareState.Hit)
            {
                if (SearchInRow(x, y))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SearchInRow(int x, int y)
        {
            int tryY = y;
            bool foundMiss = false;
            do
            {
                if (tryY < _battlefield._Size - 1 && _battlefield._Board[x, tryY + 1]._State == SquareState.Empty)
                {
                    _y = y + 1;
                    _x = x;
                    return true;
                }
                else if (tryY < _battlefield._Size - 1 && _battlefield._Board[x, tryY + 1]._State == SquareState.Miss)
                {
                    foundMiss = true;
                }
                tryY++;
            } while (tryY < _battlefield._Size && !foundMiss);

            tryY = y;
            foundMiss = false;
            do
            {
                if (tryY > 0 && _battlefield._Board[x, tryY - 1]._State == SquareState.Empty)
                {
                    _y = y - 1;
                    _x = x;
                    return true;
                }
                else if (tryY > 0 && _battlefield._Board[x, tryY - 1]._State == SquareState.Miss)
                {
                    foundMiss = true;
                }
                tryY--;
            } while (tryY > 0 && !foundMiss);
            return false;
        }

        private bool CheckHorizontalNeighbors(int x, int y)
        {
            if (x > 0 && _battlefield._Board[x - 1, y]._State == SquareState.Hit)
            {
                if (SeachInCollumn(x, y))
                {
                    return true;
                }
            }
            if (x < _battlefield._Size - 1 && _battlefield._Board[x + 1, y]._State == SquareState.Hit)
            {
                if (SeachInCollumn(x, y))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SeachInCollumn(int x, int y)
        {
            int tryX = x;
            bool foundMiss = false;
            do
            {
                if (tryX < _battlefield._Size - 1 && _battlefield._Board[tryX + 1, y]._State == SquareState.Empty)
                {
                    _x = x + 1;
                    _y = y;
                    return true;
                }
                else if (tryX < _battlefield._Size - 1 && _battlefield._Board[tryX + 1, y]._State == SquareState.Miss)
                {
                    foundMiss = true;
                }
                tryX++;
            } while (tryX < _battlefield._Size && !foundMiss);

            tryX = x;
            foundMiss = false;
            do
            {
                if (tryX > 0 && _battlefield._Board[tryX - 1, y]._State == SquareState.Empty)
                {
                    _x = x - 1;
                    _y = y;
                    return true;
                }
                else if (tryX > 0 && _battlefield._Board[tryX - 1, y]._State == SquareState.Miss)
                {
                    foundMiss = true;
                }
                tryX--;
            } while (tryX > 0 && !foundMiss);
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
