﻿namespace SchiffeVersenken.Data.Sea
{
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

        public void ShootOnSquare()
        {
            if(_State == SquareState.Empty || _State == SquareState.Blocked)
            {
                _State = SquareState.Miss;
            }
            else if (_State == SquareState.Ship)
            {
                _State = SquareState.Hit;
                _Ship?.ShipUpdate();
            }
        }

        public void SetToEmptySquare()
        {
            _State = SquareState.Empty;
            _Ship = null;
        }

        public void SetToShipSquare(Ship kreuzer)
        {
            _State = SquareState.Ship;
            _Ship = kreuzer;
        }

        public void UpdateView()
        {
            //UpdateSquare(this)
        }
    }
}