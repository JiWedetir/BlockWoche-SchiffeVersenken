using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.View

{
    public class BattlefieldPlayer: Battlefield
    {
        public BattlefieldPlayer(GameLogic game) : base(game)
        {
            CreateField();
            game.RegisterView(this);
        }
    }
}
