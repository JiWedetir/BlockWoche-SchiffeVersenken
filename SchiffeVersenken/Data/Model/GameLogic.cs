using System.Diagnostics;
using SchiffeVersenken.Data.Model.StateMachine;
using SchiffeVersenken.Data.Model.Interfaces;
using SchiffeVersenken.Data.View;

namespace SchiffeVersenken.Data.Model
{
    public class GameLogic
    {

        private IBattleShipsGameState _currentState;
        public IBattleShipsGameState CurrentState => this._currentState;
        private List<IGameView> _gameObservers = new List<IGameView>();
        public BattlefieldPlayer _BattelfieldPlayer { get; set; }
        public BattlefieldOpponent _BattelfieldOpponent { get; set; }

        public GameLogic()
        {
            TransistionToState(new PreGameState());
        }

        public void Initialize()
        {
            TransistionToState(new GameReadyState());
        }

        public void TransistionToState(IBattleShipsGameState newState)
        {
            Debug.WriteLine($"Spiellogik: Wechsel von {this._currentState} zu {newState}");
            this._currentState?.ExitState(this);
            this._currentState = newState;
            newState.EnterState(this);
            this.NotifyObservers();
            newState.AfterEnterState(this);
        }

        public void RegisterView(IGameView view)
        {
            this._gameObservers.Add(view);
        }

        public void UnregisterView(IGameView view)
        {
            if (this._gameObservers.Contains(view))
            {
                this._gameObservers.Remove(view);
            }
        }

        private void NotifyObservers()
        {
            foreach (var observer in this._gameObservers)
            {
                observer.Update(this);
            }
        }

        public void HandlePlayerInput(int x, int y)
        {
            this._currentState.HandleInput(this, x, y);
        }
    }
}
