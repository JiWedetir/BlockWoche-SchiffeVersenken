using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchiffeVersenken.Data.Model.StateMachine;

namespace SchiffeVersenken.Data.Model.Tests
{
    [TestClass()]
    public class GameLogicTests
    {
        /// <summary>
        /// Tests if the state is changed to PreGameState
        /// </summary>
        [TestMethod()]
        public void TransistionToStatePreGameStateTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            var state = new PreGameState();

            //Act
            gameLogic.TransistionToState(state);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(PreGameState), "CurrentState is not PreGameState");
            Assert.IsNotNull(gameLogic._BattlefieldPlayer, "BattlefieldPlayer is null");
            Assert.IsNotNull(gameLogic._BattlefieldOpponent, "BattlefieldOpponent is null");
            Assert.IsTrue(gameLogic._PlayerScore == gameLogic._Size * gameLogic._Size, "PlayerScore is not correct");
            Assert.IsTrue(gameLogic._OpponentShipsSet, "OpponentShipsSet is not true");
            Assert.IsNotNull(gameLogic._ComputerOpponent, "ComputerOpponent is null");
        }

        /// <summary>
        /// Tests if the state is changed to Player1TurnState
        /// </summary>
        [TestMethod()]
        public void TransistionToStatePlayer1TurnStateTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            var state = new Player1TurnState();

            //Act
            gameLogic.TransistionToState(state);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(Player1TurnState), "CurrentState is not Player1TurnState");
            Assert.IsTrue(gameLogic._Player._YourTurn, "Player._YourTurn is not true");
        }

        /// <summary>
        /// Tests if the state is changed to Player2TurnState
        /// </summary>
        [TestMethod()]
        public void TransistionToStatePlayer2TurnStateTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            var state = new Player2TurnState();

            //Act
            gameLogic.TransistionToState(state);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(Player2TurnState), "CurrentState is not Player2TurnState");
            Assert.IsTrue(gameLogic._Opponent._YourTurn, "Opponent._YourTurn is not true");
        }

        /// <summary>
        /// Tests if the state is changed to GameOverState
        /// </summary>
        [TestMethod()]
        public void TransistionToStateGameOverStateTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            var state = new GameOverState();

            //Act
            gameLogic.TransistionToState(state);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(GameOverState), "CurrentState is not GameOverState");
        }

        /// <summary>
        /// Tests if the state is changed to Player1TurnState or Player2TurnState
        /// </summary>
        [TestMethod()]
        public void SelectPlayerTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            gameLogic.TransistionToState(new PreGameState());

            //Act
            gameLogic.SelectPlayer(false, false);

            //Assert
            Assert.IsNotInstanceOfType(gameLogic.CurrentState, typeof(PreGameState), "CurrentState is still PreGameState");
        }

        /// <summary>
        /// Tests if the state is changed form Player1TurnState to Player2TurnState
        /// </summary>
        [TestMethod()]
        public void SelectPlayer1MissTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            gameLogic.TransistionToState(new Player1TurnState());

            //Act
            gameLogic.SelectPlayer(false, false);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(Player2TurnState), "CurrentState should be Player2TurnState");
        }

        /// <summary>
        /// Tests if the state is changed form Player2TurnState to Player1TurnState
        /// </summary>
        [TestMethod()]
        public void SelectPlayer2MissTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            gameLogic.TransistionToState(new Player2TurnState());

            //Act
            gameLogic.SelectPlayer(false, false);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(Player1TurnState), "CurrentState should be Player1TurnState");
        }

        /// <summary>
        /// Tests if the state is still Player1TurnState after a hit
        /// </summary>
        [TestMethod()]
        public void SelectPlayer1HitTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            gameLogic.TransistionToState(new Player1TurnState());

            //Act
            gameLogic.SelectPlayer(true, false);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(Player1TurnState), "CurrentState should still be Player1TurnState");
        }

        /// <summary>
        /// Tests if the state is still Player2TurnState after a hit
        /// </summary>
        [TestMethod()]
        public void SelectPlayer2HitTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            gameLogic.TransistionToState(new Player2TurnState());

            //Act
            gameLogic.SelectPlayer(true, false);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(Player2TurnState), "CurrentState should still be Player2TurnState");
        }

        /// <summary>
        /// Tests if the state is changed to GameOverState
        /// </summary>
        [TestMethod()]
        public void SelectPlayerGameOverTest()
        {
            //Arrange
            GameLogic gameLogic = new GameLogic();
            gameLogic.TransistionToState(new Player1TurnState());

            //Act
            gameLogic.SelectPlayer(true, true);

            //Assert
            Assert.IsInstanceOfType(gameLogic.CurrentState, typeof(GameOverState), "CurrentState is still Player1TurnState");
            Assert.IsTrue(gameLogic._Winner == "Player", $"Winner is: {gameLogic._Winner} instead of Player");
        }
    }
}