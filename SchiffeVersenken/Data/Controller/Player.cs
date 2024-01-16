using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Model.Interfaces;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.Controller
{
    public class Player: IPlayerBehaviour
    {
        private int _size;
        private List<Ship> placedShips = new List<Ship>();
        private GameLogic _game;
        public bool _YourTurn = false;

        public Player(GameLogic game)
        {
            _game = game;
            _size = game._Size;
        }

        /// <summary>
        /// Checks if the ships can be set on the board
        /// </summary>
        /// <param name="shipsToCheck">List with shipdetails of all ships to check</param>
        /// <returns>true if all ships can be set</returns>
        public bool CheckShips(List<ShipDetails> shipsToCheck)
        {
			_size = _game._Size;
			int[, ] testField = new int[_size, _size];
            foreach (var ship in shipsToCheck)
            {
                if ((ship.Orientation == Orientation.Horizontal && ship.PositionX + ship.Size > _size) || (ship.Orientation == Orientation.Vertical && ship.PositionY + ship.Size > _size))
                {
                    return false;
                }
                bool fieldOccupied = false;
                for (int i = -1; i <= ship.Size; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int posX = ship.Orientation == Orientation.Horizontal ? ship.PositionX + i : ship.PositionX + j;
                        int posY = ship.Orientation == Orientation.Vertical ? ship.PositionY + j : ship.PositionY + i;

                        if (posX >= 0 && posX < _size && posY >= 0 && posY < _size && testField[posX, posY] == 0)
                        {
                            testField[posX, posY] = 1;
                            fieldOccupied = false;
                            break;
                        }
                    }
                    if (fieldOccupied)
                    {
                        break;
                    }
                }
                if (fieldOccupied)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Sets the ships on the board
        /// </summary>
        /// <param name="shipsToSet">List with shipdeatils of the ships to set</param>
        /// <returns>true if all ships are set successfully</returns>
        public bool SetShips(List<ShipDetails> shipsToSet)
        {
            if (!CheckShips(shipsToSet))
            {
                return false;
            }
            foreach (var ship in shipsToSet)
            {
                Ship validShip = new Ship();
                placedShips.Add(validShip);
                for (int i = 0; i < ship.Size; i++)
                {
                    if (ship.Orientation == Orientation.Horizontal)
                    {
                        validShip.SetShip(_game._BattlefieldPlayer._Board[ship.PositionX + i, ship.PositionY]);
                    }
                    else
                    {
                        validShip.SetShip(_game._BattlefieldPlayer._Board[ship.PositionX, ship.PositionY + i]);
                    }
                }
                if (CheckIfAllShipsSet())
                {
                    _game.AllShipAreSet();
                    // unteres neu
                    return true;
                }
                //////////////// Wahr Fehlerhaft ////////////////////////////////////////////////////////////////////
                //else
                //{
                //    return false;
                //}
            }
            // alt return true
            return false;
        }

        /// <summary>
        /// Checks if all ships are set
        /// </summary>
        /// <returns>true if all ship are set</returns>
        public bool CheckIfAllShipsSet()
        {
            return placedShips.Count == 10;
        }

        /// <summary>
        /// Shoots at the given position
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void Shoot(int x, int y)
        {
            if(_YourTurn)
            {
                _game.HandlePlayerInput(x, y);
            }
        }
    }
}
