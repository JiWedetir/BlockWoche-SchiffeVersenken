using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;
using SchiffeVersenken.Data.Model.Interfaces;

namespace SchiffeVersenken.Data.View
{
    public abstract class Battlefield: IGameView
    {
        protected int _size;
        protected Square[,] _board;
        public Square[,] _Board { get { return _board; } }
        public int _Size { get { return _size; } }
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

        public void Shoot(int x, int y)
        {
            _Board[x, y].ShootOnSquare();
        }

        public bool CheckGameOver()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_Board[i, j]._State == SquareState.Ship)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
