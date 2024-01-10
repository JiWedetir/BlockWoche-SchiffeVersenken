using SchiffeVersenken.Data.Ship;

namespace SchiffeVersenken.Data.View

{
    public class BattlefieldPlayer: Battlefield
    {
        private int _size;
        private Square[,] _board;

        public BattlefieldPlayer(int size) : base(size)
        {
            _size = size;
            _board = new Square[_size, _size];
            for(int i = 0; i < _size; i++)
            {
                for(int j = 0; j < _size; j++)
                {
                    _board[i, j] = new Square();
                    _board[i, j].SetToEmptySquare();
                }
            }
        }
    }
}
