using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.View
{
    public class BattlefieldOpponent: Battlefield
    {
        public BattlefieldOpponent(GameLogic game) : base(game)
        {
            CreateField();
            game.RegisterView(this);
        }
    }
}
