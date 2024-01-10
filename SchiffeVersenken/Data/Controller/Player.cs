using SchiffeVersenken.Data.Model.Interfaces;
using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Ship;
using SchiffeVersenken.Data.Model;

namespace SchiffeVersenken.Data.Controller
{
    public class Player: IPlayerBehaviour
    {
        private int _size;
        private Square[,] _board;
        private List<Ship.Ship> placedShips = new List<Ship.Ship>();
        private GameLogic _game;

        public Player(GameLogic game)
        {
            _game = game;
        }
        public bool SetShip(int x, int y, bool horizontal, int length)
        {
            if ((horizontal && x + length > _size) || (!horizontal && y + length > _size))
            {
                return false;
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
                return false;
            }

            Ship.Ship kreuzer = new Ship.Ship();
            placedShips.Add(kreuzer);
            for (int i = 0; i < length; i++)
            {
                if (horizontal)
                {
                    kreuzer.SetShip(_board[x + i, y]);
                }
                else
                {
                    kreuzer.SetShip(_board[x, y + i]);
                }
            }

            return true;
            if(CheckIfAllShipsSet())
            {
                _game.AllShipAreSet();
            }
        }

        public void DeleteShip(int x, int y)
        {
            Ship.Ship kreuzer = _board[x, y]._Ship;
            kreuzer.Delete();
            placedShips.Remove(kreuzer);
        }

        public void ClearBoard()
        {
            foreach (var kreuzer in placedShips)
            {
                kreuzer.Delete();
            }
            placedShips.Clear();
        }

        public bool CheckIfAllShipsSet()
        {
            return placedShips.Count == 10;
        }

        public void SetBoardSize(int size)
        {
            _size = size;
            _game.SetSize(size);
        }
    }
}
