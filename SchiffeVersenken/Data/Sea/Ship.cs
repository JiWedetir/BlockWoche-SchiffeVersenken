namespace SchiffeVersenken.Data.Sea
{
    public class Ship
    {
        private LinkedList<Square> _shipSquares;
        public int _Length { get { return _shipSquares.Count; } }

        /// <summary>
        /// Represents a ship in the game.
        /// </summary>
        public Ship()
        {
            _shipSquares = new LinkedList<Square>();
        }

        /// <summary>
        /// Sets a ship on the specified square.
        /// </summary>
        /// <param name="square">The square to set the ship on.</param>
        public void SetShip(Square square)
        {
            _shipSquares.AddLast(square);
            square.SetToShipSquare(this);
        }

        /// <summary>
        /// Deletes the ship by setting the state of each ship square to empty and removing the ship reference.
        /// </summary>
        public void Delete()
        {
            foreach (Square shipSquare in _shipSquares)
            {
                shipSquare._State = SquareState.Empty;
                shipSquare._Ship = null;
            }
        }

        /// <summary>
        /// Updates the state of the ship based on the state of its squares.
        /// If all squares of the ship are in the "Hit" state, the ship is considered "Sunk".
        /// </summary>
        public void ShipUpdate()
        {
            if (_shipSquares.All(s => s._State == SquareState.Hit))
            {
                foreach (Square shipSquare in _shipSquares)
                {
                    shipSquare._State = SquareState.Sunk;
                }
            }
        }
    }
}
