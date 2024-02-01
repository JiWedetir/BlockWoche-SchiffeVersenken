using SchiffeVersenken.Data.ComputerPlayer;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Model.Interfaces;
using SchiffeVersenken.Data.Model.StateMachine;
using SchiffeVersenken.Data.View;
using System.Diagnostics;

namespace SchiffeVersenken.Data.Model
{
	public class GameLogic
    {
		public event Action<string> OnGameOver;

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
        public string _Winner { get; private set; }
        public ComputerDifficulty _ComputerDifficulty { get; private set; }
        public int _Size { get; private set; }
        public bool _OpponentShipsSet { get; set; }
        public bool _GameOver { get; set; }
        public int _PlayerScore { get; set; } = 0;
        public int _OpponentScore { get; set; } = 0;

        public GameLogic()
        {
            _Player = new Player(this);
            _ComputerOpponent = new ComputerOpponent(this);
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        public void Initialize()
        {
            TransistionToState(new GameReadyState());
        }

        /// <summary>
        /// Changes the current state to the new state, enters the new state and notifies all observers
        /// </summary>
        /// <param name="newState">the new state after transistion</param>
        public void TransistionToState(IBattleShipsGameState newState)
        {
            Debug.WriteLine($"Spiellogik: Wechsel von {this._currentState} zu {newState}");
            this._currentState?.ExitState(this);
            this._currentState = newState;
            newState.EnterState(this);
            //this.NotifyObservers();
            newState.AfterEnterState(this);
        }

        /// <summary>
        /// Adds a view to the observer list
        /// </summary>
        /// <param name="view"></param>
        public void RegisterView(IGameView view)
        {
            this._gameObservers.Add(view);
        }

        /// <summary>
        /// Removes a view from the observer list
        /// </summary>
        /// <param name="view"></param>
        public void UnregisterView(IGameView view)
        {
            if (this._gameObservers.Contains(view))
            {
                this._gameObservers.Remove(view);
            }
        }

        /// <summary>
        /// Notifies all observers to update
        /// </summary>
        private void NotifyObservers()
        {
            foreach (var observer in this._gameObservers)
            {
                observer.Update(this);
            }
        }

        /// <summary>
        /// Call to shoot on a field
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void HandlePlayerInput(int x, int y)
        {
            this._currentState.HandleInput(this, x, y);
        }

        /// <summary>
        /// Call if the boardsize and the difficulty is set to get to PreGameState
        /// </summary>
        /// <param name="size">board size</param>
        /// <param name="difficulty">Difficulty of the Computeropponent</param>
        public void StartPlacingShips(int size, ComputerDifficulty difficulty)
        {
            this._Size = size;
            this._ComputerDifficulty = difficulty;
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

        /// <summary>
        /// Call if the player has set all ships to get to GameReadyState
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void AllShipAreSet()
        {
            if (!_OpponentShipsSet)
            {
                throw new Exception("Gegner hat noch keine Schiffe gesetzt");
            }
            TransistionToState(new GameReadyState());
            SelectPlayer(false, false);
        }

        /// <summary>
        /// Selects the next player
        /// </summary>
        /// <param name="hit">true if a ship is hit</param>
        /// <param name="gameOver">true if all ship are hit</param>
        public void SelectPlayer(bool hit, bool gameOver)
        {
            IBattleShipsGameState nextState;
            if(_currentState is GameReadyState)
            {
                Random rnd = new Random();
                bool first = rnd.Next(2) == 0;
                if(first)
                {
                    nextState = _Player1TurnState;
                }
                else
                {
                    nextState = _Player2TurnState;
                }
            }
            else if(gameOver)
            {
                if (_currentState is Player1TurnState)
                {
                    //_Winner = _Player;
                    _Winner = "Player";
                }
                else
                {
                    //_Winner = _ComputerOpponent;
                    _Winner = "Computer";
                }
				OnGameOver?.Invoke(_Winner);
				nextState = new GameOverState();
            }
            else if(hit)
            {
                if(_currentState is Player1TurnState)
                {
                    nextState = _Player1TurnState;
                }
                else
                {
                    nextState = _Player2TurnState;
                }
            }
            else if(_currentState is Player1TurnState)
            {
                nextState = _Player2TurnState;
            }
            else
            {
                nextState = _Player1TurnState;
            }
            TransistionToState(nextState);
        }
    }
}
