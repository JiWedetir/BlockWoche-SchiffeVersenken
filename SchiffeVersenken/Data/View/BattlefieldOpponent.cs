using SchiffeVersenken.Data.Ship;

namespace SchiffeVersenken.Data.View
{
    public class BattlefieldOpponent: Battlefield
    {
        private int _size;
        public Square[,] _Board { get; set;}
        public BattlefieldOpponent(int size) : base(size)
        {
            _size = size;
            _Board = CreateField();
        }
    }
}
