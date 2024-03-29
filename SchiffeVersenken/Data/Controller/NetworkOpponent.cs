﻿using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.Controller
{
    public class NetworkOpponent : IOpponent
    {
        private GameLogic _game;
        private Square[,] _board;
        public bool _YourTurn {get; set;}

        public NetworkOpponent(GameLogic game)
        {
            _game = game;
        }
        /// <summary>
        /// Sets the ships on the opponent's battlefield based on the provided board configuration.
        /// </summary>
        /// <param name="board">The board configuration representing the ship positions.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean value indicating whether the ships were successfully set.</returns>
        public async Task<bool> SetShipAsync(int[, ] board)
        {
            _board = _game._BattlefieldOpponent._Board;
            var ships = FindShips(board);
            if (ships != null)
            {
                foreach (var ship in ships)
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
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Finds and returns a list of ShipDetails objects based on the provided board.
        /// </summary>
        /// <param name="board">The 2D array representing the game board.</param>
        /// <returns>A list of ShipDetails objects if all 10 ships are found, otherwise null.</returns>
        private List<ShipDetails> FindShips(int[, ] board)
        {
            List<ShipDetails> ships = new List<ShipDetails>();
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == 1)
                    {
                        int length = 0;
                        int k = j;
                        while (k < cols && board[i, k] == 1)
                        {
                            length++;
                            k++;
                        }

                        if (length >= 2)
                        {
                            ShipDetails ship = new ShipDetails();
                            ship.PositionX = i;
                            ship.PositionY = j;
                            ship.Size = length;
                            ship.Orientation = Orientation.Horizontal;
                            ships.Add(ship);
                        }

                        length = 0;
                        k = i;
                        while (k < rows && board[k, j] == 1)
                        {
                            length++;
                            board[k, j] = 0;
                            k++;
                        }

                        if (length >= 2)
                        {
                            ShipDetails ship = new ShipDetails();
                            ship.PositionX = i;
                            ship.PositionY = j;
                            ship.Size = length;
                            ship.Orientation = Orientation.Vertical;
                            ships.Add(ship);
                        }
                    }
                }
            }
            if (ships.Count == 10)
            {
                return ships;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the ship asynchronously.
        /// </summary>
        public Task SetShipAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Represents an asynchronous operation that shoots at the opponent.
        /// </summary>
        public Task ShootAsync()
        {
            throw new NotImplementedException();
        }

    }
}
