namespace SchiffeVersenken.Data.Sea
{
    public class Ship
    {
        private LinkedList<Square> _shipSquares;
        public Ship()
        {
            _shipSquares = new LinkedList<Square>();
        }
        public void SetShip(Square square)
        {
            _shipSquares.AddLast(square);
            square.SetToShipSquare(this);
        }

        public void Delete()
        {
            foreach (Square shipSquare in _shipSquares)
            {
                shipSquare._State = SquareState.Empty;
                shipSquare._Ship = null;
            }
        }

        public void ShipUpdate()
        {
            if (_shipSquares.All(s => s._State == SquareState.Hit))
            {
                foreach (Square shipSquare in _shipSquares)
                {
                    shipSquare._State = SquareState.Sunk;
                    shipSquare.UpdateView();
                }
            }
        }
    }
}
