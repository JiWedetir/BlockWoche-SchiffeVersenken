using SchiffeVersenken.Data.Model.Interfaces;
using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Ship;

namespace SchiffeVersenken.Data.Controller
{
    public class ComputerOpponent: IOpponent
    {
        public void SetShipRandom()
        {
            int[] shipLengths = { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
            Random randomShip = new Random();
            List<(int x, int y, bool horizontal, int length, Square square)> placedShips = new List<(int, int, bool, int, Square)>();

            foreach (int length in shipLengths)
            {
                bool shipSet = false;

                while (!shipSet)
                {
                    int x = randomShip.Next(_size);
                    int y = randomShip.Next(_size);
                    bool horizontal = randomShip.Next(2) == 0;

                    if ((horizontal && x + length > _size) || (!horizontal && y + length > _size))
                    {
                        continue;
                    }

                    bool fieldOccupied = false;
                    for (int i = -1; i <= length; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int posX = horizontal ? x + i : x + j;
                            int posY = horizontal ? y + j : y + i;

                            if (posX >= 0 && posX < _size && posY >= 0 && posY < _size && _board[posX, posY]._State != SquareState.Empty)
                            {
                                fieldOccupied = true;
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
                        if (placedShips.Count > 0)
                        {
                            var lastShip = placedShips[placedShips.Count - 1];
                            placedShips.RemoveAt(placedShips.Count - 1);
                        }
                        continue;
                    }

                    placedShips.Add((x, y, horizontal, length, _board[x, y]));


                    shipSet = true;
                }
            }
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
        }
    }
}
