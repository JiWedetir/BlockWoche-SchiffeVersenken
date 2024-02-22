using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.Controller
{
    public enum ComputerDifficulty
    {
        Dumm,
        Klug,
        Genie
    }
    public class ComputerOpponent: IOpponent
    {
        private int _size;
        private Square[,] _board;
        private GameLogic _game;
        private int[,] _tryBoard;
        private int[] shipLengths = { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
        public bool _YourTurn { get; set; }
        public ComputerOpponent(GameLogic game)
        {
            _game = game;
        }

        /// <summary>
        /// Asynchronously sets the ship on the opponent's battlefield.
        /// </summary>
        public async Task SetShipAsync()
        {
            await Task.Run(() =>
            {
                _board = _game._BattlefieldOpponent._Board;
                _size = _game._Size;
                _tryBoard = new int[_size, _size];
                int maxTries = 10;
                List<ShipDetails> placedShips = new List<ShipDetails>();

                bool success = PlaceShips(shipLengths, 0, maxTries, placedShips);
                if (success)
                {
                    _game._OpponentShipsSet = true;
                }
                else
                {
                    throw new Exception("Schiffe konnten nicht gesetzt werden");
                }
            });
        }

        /// <summary>
        /// Places ships on the game board based on the given ship lengths.
        /// </summary>
        /// <param name="shipLengths">An array of ship lengths.</param>
        /// <param name="index">The index of the current ship length being placed.</param>
        /// <param name="maxTries">The maximum number of tries to place a ship.</param>
        /// <param name="placedShips">A list of already placed ships.</param>
        /// <returns>True if all ships are successfully placed, false otherwise.</returns>
        private bool PlaceShips(int[] shipLengths, int index, int maxTries, List<ShipDetails> placedShips)
        {
            if (index == shipLengths.Length)
            {
                foreach (var ship in placedShips)
                {
                    Ship kreuzer = new Ship();
                    if (ship.Orientation == Orientation.Horizontal)
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            kreuzer.SetShip(_board[ship.PositionX + i, ship.PositionY]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            kreuzer.SetShip(_board[ship.PositionX, ship.PositionY + i]);
                        }
                    }
                }
                return true;
            }

            int shipLength = shipLengths[index];
            int tries = 0;

            while (tries < maxTries)
            {
                if (TryPlaceShip(shipLength, placedShips))
                {
                    if (PlaceShips(shipLengths, index + 1, maxTries, placedShips))
                    {

                        return true;
                    }
                    RemoveLastShip(placedShips);
                }
                tries++;
            }
            return false;
        }

        /// <summary>
        /// Tries to place a ship of the specified length on the game board.
        /// </summary>
        /// <param name="length">The length of the ship to be placed.</param>
        /// <param name="placedShips">The list of already placed ships.</param>
        /// <returns>True if the ship was successfully placed, false otherwise.</returns>
        private bool TryPlaceShip(int length, List<ShipDetails> placedShips)
        {
            List<ShipDetails> validStartPoints = new List<ShipDetails>();

            // finde all possible start points
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
                    if (CanPlaceShip(horizontal))
                    {
                        validStartPoints.Add(horizontal);
                    }
                    if (CanPlaceShip(vertical))
                    {
                        validStartPoints.Add(vertical);
                    }
                }
            }

            // Shuffle the start points
            Shuffle(validStartPoints);

            // try to place the ship
            foreach (var startPoint in validStartPoints)
            {
                if (CanPlaceShip(startPoint))
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
                            _tryBoard[startPoint.PositionX + i, startPoint.PositionY] = 1;
                        }
                        else
                        {
                            _tryBoard[startPoint.PositionX, startPoint.PositionY + i] = 1;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether a ship can be placed on the game board.
        /// </summary>
        /// <param name="ship">The ship details.</param>
        /// <returns><c>true</c> if the ship can be placed, otherwise <c>false</c>.</returns>
        private bool CanPlaceShip(ShipDetails ship)
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
                    if (_tryBoard[posX, posY] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Removes the last ship from the list of placed ships and updates the try board accordingly.
        /// </summary>
        /// <param name="placedShips">The list of placed ships.</param>
        private void RemoveLastShip(List<ShipDetails> placedShips)
        {
            if (placedShips.Count == 0)
                return;

            var lastShip = placedShips[placedShips.Count - 1];
            placedShips.RemoveAt(placedShips.Count - 1);

            for (int i = 0; i < lastShip.Size; i++)
            {
                if (lastShip.Orientation == Orientation.Horizontal)
                {
                    _tryBoard[lastShip.PositionX + i, lastShip.PositionY] = 0;
                }
                else
                {
                    _tryBoard[lastShip.PositionX, lastShip.PositionY + i] = 0;
                }
            }
        }

        /// <summary>
        /// Shuffles the elements in the specified list using the Fisher-Yates algorithm.
        /// </summary>
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

        /// <summary>
        /// Asynchronously performs the shooting action with the respective difficulty.
        /// </summary>
        public async Task ShootAsync()
        {
            if(_YourTurn)
            {
                if(_game._ComputerDifficulty == ComputerDifficulty.Dumm)
                {
                    await _game._ComputerOpponent.ShootStupidAsync();
                    _game.HandlePlayerInput(_game._ComputerOpponent._X, _game._ComputerOpponent._Y);
                }
                else if(_game._ComputerDifficulty == ComputerDifficulty.Klug)
                {
                    await _game._ComputerOpponent.ShootCleverAsync();
                    _game.HandlePlayerInput(_game._ComputerOpponent._X, _game._ComputerOpponent._Y);
                }
                else if(_game._ComputerDifficulty == ComputerDifficulty.Genie)
                {
                    await _game._ComputerOpponent.ShootIngeniousAsync();
                    _game.HandlePlayerInput(_game._ComputerOpponent._X, _game._ComputerOpponent._Y);
                }
            }
        }

        /// <summary>
        /// Sets a ship on the board asynchronously.
        /// </summary>
        /// <param name="board">The board representing the game state.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the ship was successfully set.</returns>
        public Task<bool> SetShipAsync(int[,] board)
        {
            throw new NotImplementedException();
        }
    }
}
