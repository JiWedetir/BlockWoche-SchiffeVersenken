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
            _board = CreateField();
        }
    }
}
