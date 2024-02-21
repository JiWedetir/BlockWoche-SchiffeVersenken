using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.ComputerPlayer
{
    public class IngeniousOpponent: CleverOpponent
    {
        private List<int[,]> _possibleFields;
        public IngeniousOpponent(GameLogic game) : base(game)
        {
            _size = _battlefield._Size;
        }

        /// <summary>
        /// Asynchronously shoots in an ingenious manner.
        /// </summary>
        public async Task ShootIngeniousAsync()
        {
            await ShootCleverAsync();
            if (!_cleverFieldFound)
            {
                await CreatePossiblePossitions();
            }
        }

        /// <summary>
        /// Asynchronously creates possible positions for the computer player to find the best possible position to shoot.
        /// </summary>
        public async Task CreatePossiblePossitions()
        {
            await Task.Run(() =>
            {
                _possibleFields = new List<int[,]>();
                for (int i = 0; i < 10; i++)
                {
                    CreatePossibleFields();
                }
                int[,] averageShipPlacement = GetAverageShipPlacement();
                GetHighestAverage(averageShipPlacement);
            });
        }
        
        /// <summary>
        /// Creates possible fields for ship placement.
        /// </summary>
        private void CreatePossibleFields()
        {
            List<ShipDetails> placedShips = new List<ShipDetails>();
            int[,] tryField = new int[_size, _size];
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_battlefield._Board[j, i]._State == SquareState.Empty || _battlefield._Board[j, i]._State == SquareState.Ship)
                    {
                        tryField[j, i] = 0;
                    }
                    else
                    {
                        tryField[j, i] = 1;
                    }
                }
            }
            int maxTries = 2;
            int[] shipLengths = _shipsToFinde;
            bool success = PlaceShips(shipLengths, 0, maxTries, placedShips, tryField);
            if (success)
            {
                _possibleFields.Add(tryField);
            }
        }

        /// <summary>
        /// Calculates the average ship placement based on the possible fields.
        /// </summary>
        private int[,] GetAverageShipPlacement()
        {
            int[,] averageShipPlacement = new int[_size, _size];

            foreach (int[,] field in _possibleFields)
            {
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        if (field[j, i] == 2)
                        {
                            averageShipPlacement[j, i] += 1;
                        }
                    }
                }
            }
            return averageShipPlacement;
        }

        /// <summary>
        /// Finds the highest average value in the given 2D array and sets the coordinates (_x, _y) accordingly.
        /// </summary>
        /// <param name="averageShipPlacement">The 2D array representing the average ship placement values.</param>
        private void GetHighestAverage(int[, ] averageShipPlacement)
        {
            int maxValue = 0;
            int maxI = 0;
            int maxJ = 0;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (averageShipPlacement[j, i] > maxValue)
                    {
                        maxValue = averageShipPlacement[j, i];
                        maxI = i;
                        maxJ = j;
                    }
                }
            }

            if (_battlefield._Board[maxJ, maxI]._State == SquareState.Empty || _battlefield._Board[maxJ, maxI]._State == SquareState.Ship)
            {
                _x = maxJ;
                _y = maxI;
            }
            else
            {
                int tries = 0;
                do
                {
                    for (int i = 0; i < _size; i++)
                    {
                        for (int j = 0; j < _size; j++)
                        {
                            if (averageShipPlacement[j, i] == maxValue)
                            {
                                maxI = i;
                                maxJ = j;
                                if (_battlefield._Board[maxJ, maxI]._State == SquareState.Empty || _battlefield._Board[maxJ, maxI]._State == SquareState.Ship)
                                {
                                    _x = maxJ;
                                    _y = maxI;
                                    return;
                                }
                            }
                        }
                    }
                    tries++;
                    maxValue --;
                } while (tries < 8);
                _x = maxJ;
                _y = maxI;
            }

        }

        /// <summary>
        /// Places the ships on the game board recursively.
        /// </summary>
        /// <param name="shipLengths">An array of ship lengths.</param>
        /// <param name="index">The index of the current ship length.</param>
        /// <param name="maxTries">The maximum number of tries to place a ship.</param>
        /// <param name="placedShips">A list of already placed ships.</param>
        /// <param name="tryField">The game board representation.</param>
        /// <returns>True if all ships are successfully placed, false otherwise.</returns>
        private bool PlaceShips(int[] shipLengths, int index, int maxTries, List<ShipDetails> placedShips, int[, ] tryField)
        {
            if (index == shipLengths.Length)
            {
                return true;
            }

            int shipLength = shipLengths[index];
            int tries = 0;

            while (tries < maxTries)
            {
                if (TryPlaceShip(shipLength, placedShips, tryField))
                {
                    if (PlaceShips(shipLengths, index + 1, maxTries, placedShips, tryField))
                    {
                        return true;
                    }
                    RemoveLastShip(placedShips, tryField);
                }
                tries++;
            }
            return false;
        }

        /// <summary>
        /// Tries to place a ship of the specified length on the game field.
        /// </summary>
        /// <param name="length">The length of the ship to be placed.</param>
        /// <param name="placedShips">The list of already placed ships.</param>
        /// <param name="tryField">The game field to try placing the ship on.</param>
        /// <returns>True if the ship was successfully placed, false otherwise.</returns>
        private bool TryPlaceShip(int length, List<ShipDetails> placedShips, int[, ] tryField)
        {
            List<ShipDetails> validStartPoints = new List<ShipDetails>();

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    ShipDetails horizontal = new ShipDetails
                    {
                        PositionX = x,
                        PositionY = y,
                        Orientation = Orientation.Horizontal,
                        Size = length
                    };
                    ShipDetails vertical = new ShipDetails
                    {
                        PositionX = x,
                        PositionY = y,
                        Orientation = Orientation.Vertical,
                        Size = length
                    };
                    if (CanPlaceShip(horizontal, tryField))
                    {
                        validStartPoints.Add(horizontal);
                    }
                    if (CanPlaceShip(vertical, tryField))
                    {
                        validStartPoints.Add(vertical);
                    }
                }
            }

            Shuffle(validStartPoints);

            foreach (var startPoint in validStartPoints)
            {
                if (CanPlaceShip(startPoint, tryField))
                {
                    placedShips.Add(new ShipDetails
                    {
                        PositionX = startPoint.PositionX,
                        PositionY = startPoint.PositionY,
                        Orientation = startPoint.Orientation,
                        Size = length
                    });
                    for (int i = 0; i < length; i++)
                    {
                        if (startPoint.Orientation == Orientation.Horizontal)
                        {
                            tryField[startPoint.PositionX + i, startPoint.PositionY] = 2;
                        }
                        else
                        {
                            tryField[startPoint.PositionX, startPoint.PositionY + i] = 2;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a ship can be placed on the game field.
        /// </summary>
        /// <param name="ship">The ship details.</param>
        /// <param name="tryField">The game field.</param>
        /// <returns>True if the ship can be placed, otherwise false.</returns>
        private bool CanPlaceShip(ShipDetails ship, int[, ] tryField)
        {
            if ((ship.Orientation == Orientation.Horizontal && ship.PositionX + ship.Size > _size) || (ship.Orientation == Orientation.Vertical && ship.PositionY + ship.Size > _size))
            {
                return false;
            }

            for (int i = 0; i < ship.Size; i++)
            {
                if (ship.Orientation == Orientation.Horizontal)
                {
                    if (tryField[ship.PositionX + i, ship.PositionY] != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (tryField[ship.PositionX, ship.PositionY + i] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Removes the last placed ship from the list of placed ships and updates the tryField accordingly.
        /// </summary>
        /// <param name="placedShips">The list of placed ships.</param>
        /// <param name="tryField">The tryField representing the game board.</param>
        private void RemoveLastShip(List<ShipDetails> placedShips, int[, ] tryField)
        {
            if (placedShips.Count == 0)
                return;

            var lastShip = placedShips[placedShips.Count - 1];
            placedShips.RemoveAt(placedShips.Count - 1);

            for (int i = 0; i < lastShip.Size; i++)
            {
                if (lastShip.Orientation == Orientation.Horizontal)
                {
                    tryField[lastShip.PositionX + i, lastShip.PositionY] = 0;
                }
                else
                {
                    tryField[lastShip.PositionX, lastShip.PositionY + i] = 0;
                }
            }
        }

        /// <summary>
        /// Shuffles the elements in a list using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to be shuffled.</param>
        private static void Shuffle<T>(List<T> list)
        {
            Random random = new Random();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
