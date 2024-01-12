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
        public IngeniousOpponent _Opponent { get; set; }
        public ComputerDifficulty _ComputerDifficulty { get; private set; }
        public int _Size { get; private set; }
        public bool _OpponentShipsSet { get; set; }
        public bool _GameOver { get; set; }

        public GameLogic()
        {
            _Player = new Player(this);
            _ComputerOpponent = new ComputerOpponent(this);
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
        }

        public void SetDifficulty(ComputerDifficulty difficulty)
        {
            this._ComputerDifficulty = difficulty;
            _Opponent = new IngeniousOpponent(this);
        }

        public void StartPlacingShips()
        {
            if(_Size == null)
            {
                _Size = 10;
            }
            if(_ComputerDifficulty == null)
            {
                _ComputerDifficulty = ComputerDifficulty.Klug;
            }
            TransistionToState(new PreGameState());
        }

        public void AllShipAreSet()
        {
            if (!_OpponentShipsSet)
            {
                throw new Exception("Gegner hat noch keine Schiffe gesetzt");
            }
            TransistionToState(new GameReadyState());
        }

        public void SelectPlayer(bool hit, bool gameOver)
        {
            if(_currentState is GameReadyState)
            {
                Random rnd = new Random();
                bool first = rnd.Next(2) == 0;
                if(first)
                {
                    TransistionToState(_Player1TurnState);
                }
                else
                {
                    TransistionToState(_Player2TurnState);
                }
            }
            else if(gameOver)
            {
                TransistionToState(new GameOverState());
            }
            else if(hit)
            {
                if(_currentState is Player1TurnState)
                {
                    TransistionToState(_Player1TurnState);
                }
                else
                {
                    TransistionToState(_Player2TurnState);
                }
            }
            else if(_currentState is Player1TurnState)
            {
                TransistionToState(_Player2TurnState);
            }
            else
            {
                TransistionToState(_Player1TurnState);
            }
        }
    }
}
