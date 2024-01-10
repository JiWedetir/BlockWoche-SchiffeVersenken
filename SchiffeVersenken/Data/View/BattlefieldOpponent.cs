using SchiffeVersenken.Data.Ship;

namespace SchiffeVersenken.Data.View
{
    public class BattlefieldOpponent: Battlefield
    {
        public BattlefieldOpponent(int size) : base(size)
        {
            _size = size;
            CreateField();
        }
    }
}
