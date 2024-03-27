using SchiffeVersenken.Data.ComputerPlayer;
using SchiffeVersenken.Data.Controller;
using SchiffeVersenken.Data.Model.StateMachine;
using SchiffeVersenken.Data.View;
using SchiffeVersenken.DatabaseEF.Database;
using System.Diagnostics;

namespace SchiffeVersenken.Data.Model
{
    public class GameLogic
    {
		public event Action<string>? OnGameOver;

		private IBattleShipsGameState? _currentState;
        public IBattleShipsGameState? CurrentState => this._currentState;
        public Player? _Player { get; set; }
        public IOpponent? _Opponent { get; set; }
        public Battlefield? _BattlefieldPlayer { get; set; }
        public Battlefield? _BattlefieldOpponent { get; set; }
        public Player1TurnState? _Player1TurnState { get; set; }
        public Player2TurnState? _Player2TurnState { get; set; }
        public string _Winner { get; private set; } = string.Empty;
        public string _Looser { get; private set; } = string.Empty;
        public IngeniousOpponent? _ComputerOpponent { get; set; }
        public ComputerDifficulty _ComputerDifficulty { get; private set; } = ComputerDifficulty.Klug;
        public int _Size { get; private set; } = 10;
        public bool _OpponentShipsSet { get; set; } = false;
        public bool _GameOver { get; set; } = false;
        public int _PlayerScore { get; set; } = 0;
        public int _TurnsPlayed { get; set; } = 0;
        public DateTime _GameStart { get; set; } = DateTime.Now;
        public bool _LocalGame { get; private set; } = true;

        public GameLogic()
        {
            _Player = new Player(this);
            if(_LocalGame)
            {
                _Opponent = new ComputerOpponent(this);
            }
            else
            {
                _Opponent = new NetworkOpponent(this);
            }
            if(UserManagement._Player == null)
            {
                _ = UserManagement.SetDefaultPlayer();
            }
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
            newState.AfterEnterState(this);
        }

        /// <summary>
        /// Call to shoot on a field
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void HandlePlayerInput(int x, int y)
        {
            this._currentState?.HandleInput(this, x, y);
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
            _ = UserManagement.SetComputerOpponent(_ComputerDifficulty);
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
            _Player1TurnState = new Player1TurnState();
            _Player2TurnState = new Player2TurnState();
            Debug.WriteLine("Spiellogik: Alle Schiffe gesetzt, Spiel ist ready");
            SelectPlayer(false, false);
        }

        /// <summary>
        /// Selects the next player
        /// </summary>
        /// <param name="hit">true if a ship is hit</param>
        /// <param name="gameOver">true if all ship are hit</param>
        public void SelectPlayer(bool hit, bool gameOver)
        {
            _TurnsPlayed++;
            IBattleShipsGameState nextState;
            if(_currentState is PreGameState)
            {
                Random rnd = new Random();
                bool first = rnd.Next(2) == 0;
                _GameStart = DateTime.Now;
                if(first)
                {
                    nextState = _Player1TurnState ?? throw new ArgumentNullException(nameof(_Player1TurnState));
                }
                else
                {
                    nextState = _Player2TurnState ?? throw new ArgumentNullException(nameof(_Player2TurnState));
                }
            }
            else if(gameOver)
            {
                if (_currentState is Player1TurnState)
                {
                    _Winner = UserManagement._Player.Name;
                    _Looser = UserManagement._Opponent.Name;
                }
                else
                {
                    _Winner = UserManagement._Opponent.Name;
                    _Looser = UserManagement._Player.Name;
                }
				OnGameOver?.Invoke(_Winner);
				nextState = new GameOverState();
            }
            else if(hit)
            {
                if(_currentState is Player1TurnState)
                {
                    nextState = _Player1TurnState ?? throw new ArgumentNullException(nameof(_Player1TurnState));
                }
                else
                {
                    nextState = _Player2TurnState ?? throw new ArgumentNullException(nameof(_Player2TurnState));
                }
            }
            else if(_currentState is Player1TurnState)
            {
                nextState = _Player2TurnState ?? throw new ArgumentNullException(nameof(_Player2TurnState));
            }
            else
            {
                nextState = _Player1TurnState ?? throw new ArgumentNullException(nameof(_Player1TurnState));
            }
            TransistionToState(nextState);
        }
    }
}
