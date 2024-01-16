namespace SchiffeVersenken.Data.Sea
{
    /// <summary>
    /// Enum for the state of a square: Empty, Miss, Ship, Hit, Sunk, Blocked
    /// </summary>
    public enum SquareState
    {
        Empty,
        Miss,
        Ship,
        Hit,
        Sunk,
        Blocked
    }

    public class Square
    {
        public SquareState _State { get; set; } = SquareState.Empty;
        public Ship ?_Ship { get; set; }

        /// <summary>
        /// Shoots on a square and changes the state of the square
        /// </summary>
        /// <returns></returns>
        public async Task ShootOnSquareAsync()
        {
            await Task.Run(() => { 
            if (_State == SquareState.Empty || _State == SquareState.Blocked)
            {
                _State = SquareState.Miss;
            }
            else if (_State == SquareState.Ship)
            {
                _State = SquareState.Hit;
                _Ship?.ShipUpdate();
            }});
        }

        /// <summary>
        /// Sets the square to an empty square to initialize the board
        /// </summary>
        public void SetToEmptySquare()
        {
            _State = SquareState.Empty;
            _Ship = null;
        }

        /// <summary>
        /// Sets the square to a ship square
        /// </summary>
        /// <param name="kreuzer">the ship to set on the square</param>
        public void SetToShipSquare(Ship kreuzer)
        {
            _State = SquareState.Ship;
            _Ship = kreuzer;
        }
    }
}
