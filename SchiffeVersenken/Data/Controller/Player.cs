using SchiffeVersenken.Data.Model.Interfaces;
using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Ship;

namespace SchiffeVersenken.Data.Controller
{
    public class Player: IPlayerBehaviour
    {
        private int _size;
        private Square[,] _board;
        private List<Kreuzer> placedShips = new List<Kreuzer>();
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

            Kreuzer kreuzer = new Kreuzer();
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
        }

        public void DeleteShip(int x, int y)
        {
            Kreuzer kreuzer = _board[x, y]._Ship;
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
        }
    }
}
