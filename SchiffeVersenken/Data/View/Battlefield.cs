using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Ship;
using SchiffeVersenken.Data.Model.Interfaces;

namespace SchiffeVersenken.Data.View
{
    public abstract class Battlefield: IGameView
    {
        protected int _size;
        protected Square[,] _board;
        public Square[,] _Board { get { return _board; } }
        public Battlefield(int size)
        {
            _size = size;
        }

        public void Update(GameLogic game)
        {
            throw new NotImplementedException();
        }

        public void CreateField()
        {
            _board = new Square[_size, _size];
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _board[i, j] = new Square();
                    _board[i, j].SetToEmptySquare();
                }
            }
        }
    }
}
