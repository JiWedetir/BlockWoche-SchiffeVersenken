namespace SchiffeVersenken.Data.Ship
{
    public enum SquareState
    {
        Empty,
        Miss,
        Ship,
        Hit,
        Sunk
    }

    public class Square
    {
        public SquareState _State { get; set; } = SquareState.Empty;
        public Ship ?_Ship { get; set; }

        public void UpdateSquare()
        {
            if(_State == SquareState.Empty)
            {
                _State = SquareState.Miss;
            }
            else if (_State == SquareState.Ship)
            {
                _State = SquareState.Hit;
                _Ship?.ShipUpdate();
            }

        }

        public void SetToShipSquare(Ship ship)
        {
            _State = SquareState.Ship;
            _Ship = ship;
            _Ship.AddShipSquare(this);
        }

        public void UpdateView()
        {
            //UpdateSquare(this)
        }
    }
}
