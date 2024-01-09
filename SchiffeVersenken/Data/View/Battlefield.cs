using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Ship;
using SchiffeVersenken.Data.Model.Interfaces;

namespace SchiffeVersenken.Data.View
{
    public abstract class Battlefield: IGameView
    {
        private int _size;
        public Battlefield(int size)
        {
            _size = size;
        }

        public void Update(GameLogic game)
        {
            throw new NotImplementedException();
        }
    }
}
