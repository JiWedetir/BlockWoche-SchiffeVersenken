
using SchiffeVersenken.Data.Model;

namespace SchiffeVersenken.Data
{
    public class GameLogicService
    {
        public GameLogic Game { get; private set; }

        public void CreateNewGame()
        {
            Game = new GameLogic();
		}
    }
}
