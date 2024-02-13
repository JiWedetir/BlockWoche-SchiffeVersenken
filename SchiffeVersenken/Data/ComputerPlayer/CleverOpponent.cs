using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.ComputerPlayer
{
    public abstract class CleverOpponent: StupidOpponent
    {
        protected bool _cleverFieldFound;
        protected int _size;
        public CleverOpponent(GameLogic game) : base(game)
        {
        }

        /// <summary>
        /// Shoots at a random square, if it hits it will shoot at the adjacent squares and if it hits 
        /// again it will detect the direction of the ship and shoot in that direction
        /// </summary>
        /// <returns></returns>
        public async Task ShootCleverAsync()
        {
            _cleverFieldFound = false;
            bool shipSunk = false;
            int lengthSunkShip = 0;
            await Task.Run(() =>
            {
                for (int i = _shootHistory.Count - 1; i >= 0; i--)
                {
                    if (_shootHistory[i].hit == true && _shootHistory[i].sunk == false)
                    {
                        int x = _shootHistory[i].x;
                        int y = _shootHistory[i].y;
                        if (_battlefield._Board[x, y]._State == SquareState.Hit)
                        {
                            if (CheckAdjacentSquares(x, y))
                            {
                                _cleverFieldFound = true;
                                return;
                            }
                        }
                        else if (_battlefield._Board[x, y]._State == SquareState.Sunk)
                        {
                            MarkAdjacentSquares(x, y);
                            _shootHistory[i] = (_shootHistory[i].x, _shootHistory[i].y, _shootHistory[i].hit, true);
                            _cleverFieldFound = false;
                            lengthSunkShip = _battlefield._Board[x, y]._Ship._Length;
                            shipSunk = true;
                        }
                    }
                }
                if (shipSunk)
                {
                    RemoveShipFromShipToFind(lengthSunkShip);
                }
            });
            if (!_cleverFieldFound)
            {
                await ShootStupidAsync();
            }
        }

        /// <summary>
        /// Marks the adjacent squares of a sunk ship as blocked
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        private void MarkAdjacentSquares(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

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

        /// <summary>
        /// Checks if there are adjacent squares to shoot at
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
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
                if(y > 0 && _battlefield._Board[x, y - 1]._State == SquareState.Empty || y > 0 && _battlefield._Board[x, y - 1]._State == SquareState.Ship)
                {
                    _y = y - 1;
                    _x = x;
                    return true;
                }
                else if (y < _battlefield._Size - 1 && _battlefield._Board[x, y + 1]._State == SquareState.Empty || y < _battlefield._Size - 1 && _battlefield._Board[x, y + 1]._State == SquareState.Ship)
                {
                    _y = y + 1;
                    _x = x;
                    return true;
                }
                else if (x > 0 && _battlefield._Board[x - 1, y]._State == SquareState.Empty || x > 0 && _battlefield._Board[x - 1, y]._State == SquareState.Ship)
                {
                    _x = x - 1;
                    _y = y;
                    return true;
                }
                else if (x < _battlefield._Size - 1 && _battlefield._Board[x + 1, y]._State == SquareState.Empty || x < _battlefield._Size - 1 && _battlefield._Board[x + 1, y]._State == SquareState.Ship)
                {
                    _x = x + 1;
                    _y = y;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if there are vertical neighbors to shoot at
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
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

        /// <summary>
        /// Searches for a square to shoot at in a row
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        private bool SearchInRow(int x, int y)
        {
            int tryY = y;
            bool foundMiss = false;
            do
            {
                if (tryY < _battlefield._Size - 1)
                {
                    var state = _battlefield._Board[x, tryY + 1]._State;
                    if (state == SquareState.Empty || state == SquareState.Ship)
                    {
                        _y = tryY + 1;
                        _x = x;
                        return true;
                    }
                    else if (state == SquareState.Miss)
                    {
                        foundMiss = true;
                    }
                }
                tryY++;
            } while (tryY < _battlefield._Size && !foundMiss);

            tryY = y;
            foundMiss = false;
            do
            {
                if (tryY > 0)
                {
                    var state = _battlefield._Board[x, tryY - 1]._State;
                    if (state == SquareState.Empty || state == SquareState.Ship)
                    {
                        _y = tryY - 1;
                        _x = x;
                        return true;
                    }
                    else if (state == SquareState.Miss)
                    {
                        foundMiss = true;
                    }
                }
                tryY--;
            } while (tryY > 0 && !foundMiss);
            return false;
        }

        /// <summary>
        /// Checks if there are horizontal neighbors to shoot at
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
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

        /// <summary>
        /// Searches for a square to shoot at in a collumn
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        private bool SeachInCollumn(int x, int y)
        {
            int tryX = x;
            bool foundMiss = false;
            do
            {
                if (tryX < _battlefield._Size - 1)
                {
                    var state = _battlefield._Board[tryX + 1, y]._State;
                    if (state == SquareState.Empty || state == SquareState.Ship)
                    {
                        _x = tryX + 1;
                        _y = y;
                        return true;
                    }
                    else if (state == SquareState.Miss)
                    {
                        foundMiss = true;
                    }
                }
                tryX++;
            } while (tryX < _battlefield._Size && !foundMiss);

            tryX = x;
            foundMiss = false;
            do
            {
                if (tryX > 0)
                {
                    var state = _battlefield._Board[tryX - 1, y]._State;
                    if (state == SquareState.Empty || state == SquareState.Ship)
                    {
                        _x = tryX - 1;
                        _y = y;
                        return true;
                    }
                    else if (state == SquareState.Miss)
                    {
                        foundMiss = true;
                    }
                }
                tryX--;
            } while (tryX > 0 && !foundMiss);
            return false;
        }

        /// <summary>
        /// Removes the ship from the list of ships to find for IngeniousOpponent
        /// </summary>
        /// <param name="length"></param>
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
