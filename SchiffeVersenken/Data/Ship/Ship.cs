namespace SchiffeVersenken.Data.Ship
{
    public class Ship
    {
        private LinkedList<Square> _shipSquares;

        public void AddShipSquare(Square square)
        {
            _shipSquares.AddLast(square);
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
