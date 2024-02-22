using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.View
{
    public class Battlefield
    {
        public event Action<SquareState>? OnPlayerAction;
		public event Action<string>? OnGameOver;

        protected int _size = 10;
        protected Square[,]? _board;
        public Square[,] _Board { get { return _board ?? throw new ArgumentNullException(nameof(_board)); } }
        public int _Size { get { return _size; } }
        public Battlefield(GameLogic game)
        {
            _size = game._Size;
            CreateField();
        }

        /// <summary>
        /// Creates the field for the game and sets all squares to empty
        /// </summary>
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

        /// <summary>
        /// shoots on the square and returns true if it was a hit
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        public async Task<bool> ShootAsync(int x, int y)
        {
            await _Board[x, y].ShootOnSquareAsync();

            OnPlayerAction?.Invoke(_Board[x, y]._State);

            if (_Board[x, y]._State == SquareState.Hit || _Board[x, y]._State == SquareState.Sunk)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if all ships are sunk
        /// </summary>
        /// <returns>true if the game is over</returns>
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
