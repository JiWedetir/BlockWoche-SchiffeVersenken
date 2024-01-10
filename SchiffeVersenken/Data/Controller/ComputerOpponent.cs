using SchiffeVersenken.Data.Model.Interfaces;
using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Ship;
using SchiffeVersenken.Data.Model;
using System;

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
        public ComputerOpponent(GameLogic game)
        {
            _game = game;
        }

        public void SetShipRandom()
        {
            _board = _game._Player2Board._Board;
            _size = _game._Size;
            _tryBoard = new int[_size, _size];
            int[] shipLengths = { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
            int maxTries = 1000;
            List<(int x, int y, bool horizontal, int length)> placedShips = new List<(int x, int y, bool horizontal, int length)>();

            bool success = PlaceShips(shipLengths, 0, maxTries, placedShips);
            if (!success)
            {
                throw new Exception("Could not place ships");
            }
        }

        private bool TryPlaceShip(int length, List<(int x, int y, bool horizontal, int length)> placedShips)
        {
            List<(int x, int y, bool horizontal)> validStartPoints = new List<(int x, int y, bool horizontal)>();

            // Finden Sie alle gültigen Startpunkte, die genug Platz für das Schiff bieten
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (CanPlaceShip(x, y, true, length))
                    {
                        validStartPoints.Add((x, y, true));
                    }
                    if (CanPlaceShip(x, y, false, length))
                    {
                        validStartPoints.Add((x, y, false));
                    }
                }
            }

            // Mischen Sie die Liste der gültigen Startpunkte, um Zufälligkeit zu gewährleisten
            Shuffle(validStartPoints);

            // Versuchen Sie, das Schiff an einem der gültigen Startpunkte zu platzieren
            foreach (var startPoint in validStartPoints)
            {
                if (CanPlaceShip(startPoint.x, startPoint.y, startPoint.horizontal, length))
                {
                    placedShips.Add((startPoint.x, startPoint.y, startPoint.horizontal, length));
                    for (int i = 0; i < length; i++)
                    {
                        if (startPoint.horizontal)
                        {
                            _tryBoard[startPoint.x + i, startPoint.y] = 1;
                        }
                        else
                        {
                            _tryBoard[startPoint.x, startPoint.y + i] = 1;
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        private bool PlaceShips(int[] shipLengths, int index, int maxTries, List<(int x, int y, bool horizontal, int length)> placedShips)
        {
            if (index == shipLengths.Length) // Alle Schiffe wurden platziert
            {
                foreach (var ship in placedShips)
                {
                    Kreuzer kreuzer = new Kreuzer();
                    if (ship.horizontal)
                    {
                        for (int i = 0; i < ship.length; i++)
                        {
                            kreuzer.SetShip(_board[ship.x + i, ship.y]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ship.length; i++)
                        {
                            kreuzer.SetShip(_board[ship.x, ship.y + i]);
                        }
                    }
                }
                return true;
            }

            int shipLength = shipLengths[index];
            int tries = 0;

            while (tries < maxTries)
            {
                if (TryPlaceShip(shipLength, placedShips)) // Versuchen Sie, das Schiff zu platzieren
                {
                    if (PlaceShips(shipLengths, index + 1, maxTries, placedShips)) // Versuchen Sie, die restlichen Schiffe zu platzieren
                    {

                        return true;
                    }
                    RemoveLastShip(placedShips);
                }
                tries++;
            }
            return false;

        }

        private bool CanPlaceShip(int x, int y, bool horizontal, int length)
        {
            if ((horizontal && x + length > _size) || (!horizontal && y + length > _size))
            {
                return false;
            }

            int startX = Math.Max(0, x - 1);
            int startY = Math.Max(0, y - 1);
            int endX = Math.Min(_size - 1, horizontal ? x + length : x + 1);
            int endY = Math.Min(_size - 1, horizontal ? y + 1 : y + length);

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

        private void RemoveLastShip(List<(int x, int y, bool horizontal, int length)> placedShips)
        {
            if (placedShips.Count == 0)
                return;

            var lastShip = placedShips[placedShips.Count - 1];
            placedShips.RemoveAt(placedShips.Count - 1);

            for (int i = 0; i < lastShip.length; i++)
            {
                if (lastShip.horizontal)
                {
                    _tryBoard[lastShip.x + i, lastShip.y] = 0;
                }
                else
                {
                    _tryBoard[lastShip.x, lastShip.y + i] = 0;
                }
            }
        }

        private static void Shuffle<T>(List<T> list)
        {
            Random randomShip = new Random();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = randomShip.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }


        //public void SetShipRandom()
        //{
        //    _board = _game._Player2Board._Board;
        //    _size = _game._Size;
        //    int[] shipLengths = { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
        //    Random randomShip = new Random();
        //    int[,] tryBoard = new int[_size, _size];
        //    List<(int x, int y, bool horizontal, int length, Square square)> placedShips = new List<(int, int, bool, int, Square)>();
        //    bool allShipsPlaced = false;
        //    int maxTries = 10000000;

        //    while (!allShipsPlaced)
        //    {
        //        Array.Clear(tryBoard, 0, tryBoard.Length);

        //        foreach (int length in shipLengths)
        //        {
        //            bool shipSet = false;
        //            int tries = 0;

        //            while (!shipSet && tries < maxTries)
        //            {
        //                int x = randomShip.Next(_size);
        //                int y = randomShip.Next(_size);
        //                bool horizontal = randomShip.Next(2) == 0;

        //                if ((horizontal && x + length > _size) || (!horizontal && y + length > _size))
        //                {
        //                    tries++;
        //                    continue;
        //                }

        //                bool fieldOccupied = false;
        //                for (int i = -1; i <= length; i++)
        //                {
        //                    for (int j = -1; j <= 1; j++)
        //                    {
        //                        int posX = horizontal ? x + i : x + j;
        //                        int posY = horizontal ? y + j : y + i;

        //                        if (posX >= 0 && posX < _size && posY >= 0 && posY < _size && tryBoard[posX, posY] != 0)
        //                        {
        //                            fieldOccupied = true;
        //                            break;
        //                        }
        //                    }
        //                    if (fieldOccupied)
        //                    {
        //                        break;
        //                    }
        //                }

        //                if (fieldOccupied)
        //                {
        //                    if (placedShips.Count > 0)
        //                    {
        //                        var lastShip = placedShips[placedShips.Count - 1];
        //                        placedShips.RemoveAt(placedShips.Count - 1);
        //                        for (int i = 0; i < lastShip.length; i++)
        //                        {
        //                            if (lastShip.horizontal)
        //                            {
        //                                tryBoard[lastShip.x + i, lastShip.y] = 0;
        //                            }
        //                            else
        //                            {
        //                                tryBoard[lastShip.x, lastShip.y + i] = 0;
        //                            }
        //                        }
        //                    }
        //                    tries++;
        //                    continue;
        //                }

        //                placedShips.Add((x, y, horizontal, length, _board[x, y]));
        //                for (int i = 0; i < length; i++)
        //                {
        //                    if (horizontal)
        //                    {
        //                        tryBoard[x + i, y] = 1;
        //                    }
        //                    else
        //                    {
        //                        tryBoard[x, y + i] = 1;
        //                    }
        //                }

        //                shipSet = true;
        //            }
        //            if(tries >= maxTries)
        //            {
        //                break;
        //            }
        //        }
        //        if (placedShips.Count == shipLengths.Length)
        //        {
        //            allShipsPlaced = true;
        //        }
        //    }
        //    foreach (var ship in placedShips)
        //    {
        //        Kreuzer kreuzer = new Kreuzer();
        //        if (ship.horizontal)
        //        {
        //            for (int i = 0; i < ship.length; i++)
        //            {
        //                kreuzer.SetShip(_board[ship.x + i, ship.y]);
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < ship.length; i++)
        //            {
        //                kreuzer.SetShip(_board[ship.x, ship.y + i]);
        //            }
        //        }
        //    }
        //}


        public void SetDificulty(ComputerDifficulty difficulty)
        {
            throw new NotImplementedException();
        }
    }
}
