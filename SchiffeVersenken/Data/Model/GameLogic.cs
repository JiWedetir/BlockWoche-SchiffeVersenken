using System.Diagnostics;
using SchiffeVersenken.Data.Model.StateMachine;
using SchiffeVersenken.Data.Model.Interfaces;
using SchiffeVersenken.Data.View;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.ComputerPlayer;

namespace SchiffeVersenken.Data.Model
{
    public class GameLogic
    {

        private IBattleShipsGameState _currentState;
        public IBattleShipsGameState CurrentState => this._currentState;
        private List<IGameView> _gameObservers = new List<IGameView>();
        public Player _Player { get; set; }
        public ComputerOpponent _ComputerOpponent { get; set; }
        public BattlefieldPlayer _BattlefieldPlayer { get; set; }
        public BattlefieldOpponent _BattlefieldOpponent { get; set; }
        public Player1TurnState _Player1TurnState { get; set; }
        public Player2TurnState _Player2TurnState { get; set; }
        public StupidOpponent _StupidOpponent { get; set; }
        public CleverOpponent _CleverOpponent { get; set; }
        public IngeniousOpponent _IngeniousOpponent { get; set; }
        public int _Size { get; private set; }

        public GameLogic()
        {
            _Player = new Player(this);
            _ComputerOpponent = new ComputerOpponent(this);
            SetSize(9);
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

        public void SetSize(int size)
        {
            this._Size = size;
            TransistionToState(new PreGameState());
        }

        public void AllShipAreSet()
        {
            TransistionToState(new GameReadyState());
        }

        public void StartGame()
        {
            TransistionToState(new GameReadyState());
        }

        public void PlayerOneTurn()
        {
            TransistionToState(_Player1TurnState);
        }

        public void PlayerTwoTurn()
        {
            TransistionToState(_Player2TurnState);
        }

        public void GameOverState()
        {
            TransistionToState(new GameOverState());
        }

        public void SelectPlayer()
        {
            if(_currentState is GameReadyState)
            {
                Random rnd = new Random();
                bool first = rnd.Next(2) == 0;
                if(first)
                {
                    PlayerOneTurn();
                }
                else
                {
                    PlayerTwoTurn();
                }
            }
            else if(_currentState is Player1TurnState)
            {
                PlayerTwoTurn();
            }
            else
            {
                PlayerOneTurn();
            }
        }

    }
}
