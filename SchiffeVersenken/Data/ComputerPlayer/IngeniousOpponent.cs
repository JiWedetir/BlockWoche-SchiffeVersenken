using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Sea;
using Android.Health.Connect.DataTypes.Units;

namespace SchiffeVersenken.Data.ComputerPlayer
{
    public class IngeniousOpponent: CleverOpponent
    {
        private List<ShipDetails> _possiblePositions = new List<ShipDetails>();
        private List<int[,]> _possibleFields;
        public IngeniousOpponent(Battlefield battlefield, ComputerOpponent computer) : base(battlefield, computer)
        {
            _size = battlefield._Size;
        }

        public void ShootIngenious()
        {
            ShootClever();
            if (!_cleverFieldFound)
            {
                CreatePossiblePossitions();
            }
        }

        public void CreatePossiblePossitions()
        {
            int[, ] _tryField = new int[_size, _size];
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_battlefield._Board[i, j]._State == SquareState.Empty)
                    {
                        _tryField[i, j] = 0;
                    }
                    else
                    {
                        _tryField[i, j] = 1;
                    }
                }
            }
            _possibleFields = new List<int[, ]>();
            for (int i = 0; i < 10; i++)
            {
                CreatePossibleFields(_tryField);
            }
            int[,] averageShipPlacement = GetAverageShipPlacement(_possibleFields);
            GetHighestAverage(averageShipPlacement);
        }

        private void CreatePossibleFields(int[, ] tryField)
        {
            List<ShipDetails> placedShips = new List<ShipDetails>();
            int maxTries = 10;
            int[] shipLengths = _shipsToFinde;
            bool success = PlaceShips(shipLengths, 0, maxTries, placedShips, tryField);
            if (success)
            {
                _possibleFields.Add(tryField);
            }
        }

        private int[,] GetAverageShipPlacement(List<int[, ]> possibleFields)
        {
            int[,] averageShipPlacement = new int[_size, _size];

            foreach (int[,] field in _possibleFields)
            {
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        if (field[i, j] == 2)
                        {
                            averageShipPlacement[i, j] += 1;
                        }
                    }
                }
            }
            return averageShipPlacement;
        }

        private void GetHighestAverage(int[, ] averageShipPlacement)
        {
            int maxValue = 0;
            int maxI = 0;
            int maxJ = 0;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (averageShipPlacement[i, j] > maxValue)
                    {
                        maxValue = averageShipPlacement[i, j];
                        maxI = i;
                        maxJ = j;
                    }
                }
            }
            _x = maxI;
            _y = maxJ;
        }

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

        private bool CanPlaceShip(ShipDetails ship, int[, ] tryField)
        {
            if ((ship.Orientation == Orientation.Horizontal && ship.PositionX + ship.Size > _size) || (ship.Orientation == Orientation.Vertical && ship.PositionY + ship.Size > _size))
            {
                return false;
            }
            bool horizontal = ship.Orientation == Orientation.Horizontal;

            int startX = Math.Max(0, ship.PositionX - 1);
            int startY = Math.Max(0, ship.PositionY - 1);
            int endX = Math.Min(_size - 1, horizontal ? ship.PositionX + ship.Size : ship.PositionX + 1);
            int endY = Math.Min(_size - 1, horizontal ? ship.PositionY + 1 : ship.PositionY + ship.Size);

            for (int posX = startX; posX <= endX; posX++)
            {
                for (int posY = startY; posY <= endY; posY++)
                {
                    if (tryField[posX, posY] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

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
