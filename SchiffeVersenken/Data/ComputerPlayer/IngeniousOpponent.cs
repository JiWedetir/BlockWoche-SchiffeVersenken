using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.ComputerPlayer
{
    public class IngeniousOpponent: CleverOpponent
    {
        private List<(int x, int y, bool horizontal)> possiblePositions = new List<(int x, int y, bool horizontal)>();
        public IngeniousOpponent(Battlefield battlefield, ComputerOpponent computer) : base(battlefield, computer)
        {
        }

        public void ShootIngenious()
        {
            ShootClever();
            if (!_cleverFieldFound)
            {
                CreatePossiblePossitions();
            }
        }

        public void CreatePossiblePossitions()
        {
            //Try field erstellen mit allen squares die nicht empty sind auf 1 setzen
            //mit dem randomSetalgorhytmus alle möglichen positionen erstellen
            //diese dann in eine liste packen
            //dann mit dem randomsetalgorhytmus eine position auswählen
        }
    }
}
