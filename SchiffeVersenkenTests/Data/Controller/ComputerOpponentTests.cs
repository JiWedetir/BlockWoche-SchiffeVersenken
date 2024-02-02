using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchiffeVersenken.Data.ComputerPlayer;
using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;
using SchiffeVersenken.Data.View;

namespace SchiffeVersenken.Data.Controller.Tests
{
    [TestClass()]
    public class ComputerOpponentTests
    {

        /// <summary>
        /// Tests if the ships are set correctly
        /// </summary>
        [TestMethod()]
        public void SetShipAsyncTest()
        {
            //Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            var battlefield = new Battlefield(game.Object);
            battlefield.CreateField();
            game.Setup(g => g._BattlefieldOpponent).Returns(battlefield);
            var computerOpponent = new ComputerOpponent(game.Object);

            //Act
            computerOpponent.SetShipAsync().Wait();

            //Assert
            Assert.IsTrue(game.Object._OpponentShipsSet, "bool OpponentShipSet is not set true");
            int shipSquareCount = 0;
            for (int i = 0; i < battlefield._Size; i++)
            {
                for (int j = 0; j < battlefield._Size; j++)
                {
                    if (battlefield._Board[i, j]._State == Sea.SquareState.Ship)
                    {
                        shipSquareCount++;
                    }
                }
            }
            Assert.IsTrue(shipSquareCount == 30, $"Computer set {shipSquareCount}/30 ShipSquares!");
        }

        /// <summary>
        /// Tests if the stupid computer shoots on the field and misses
        /// </summary>
        [TestMethod()]
        public void ShootAsyncStupidTest()
        {
            //Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            var battlefield = new Battlefield(game.Object);
            battlefield.CreateField();
            game.Setup(g => g._BattlefieldPlayer).Returns(battlefield);
            var computerOpponent = new ComputerOpponent(game.Object);
            computerOpponent._YourTurn = true;
            game.Setup(g => g._ComputerDifficulty).Returns(ComputerDifficulty.Dumm);
            game.Setup(game => game._ComputerOpponent).Returns(new IngeniousOpponent(game.Object));

            //Act
            computerOpponent.ShootAsync().Wait();
            int x = game.Object._ComputerOpponent._X;
            int y = game.Object._ComputerOpponent._Y;

            //Assert
            Assert.IsTrue(game.Object._BattlefieldPlayer._Board[x, y]._State == SquareState.Miss, $"Squarestate did not change to Miss x:{x} y:{y}");
        }

        /// <summary>
        /// Tests if the clever computer shoots on the field and misses
        /// </summary>
        [TestMethod()]
        public void ShootAsyncCleverTest()
        {
            //Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            var battlefield = new Battlefield(game.Object);
            battlefield.CreateField();
            game.Setup(g => g._BattlefieldPlayer).Returns(battlefield);
            var computerOpponent = new ComputerOpponent(game.Object);
            computerOpponent._YourTurn = true;
            game.Setup(g => g._ComputerDifficulty).Returns(ComputerDifficulty.Klug);
            game.Setup(game => game._ComputerOpponent).Returns(new IngeniousOpponent(game.Object));

            //Act
            computerOpponent.ShootAsync().Wait();
            int x = game.Object._ComputerOpponent._X;
            int y = game.Object._ComputerOpponent._Y;

            //Assert
            Assert.IsTrue(game.Object._BattlefieldPlayer._Board[x, y]._State == SquareState.Miss, $"Squarestate did not change to Miss x:{x} y:{y}");
        }

        /// <summary>
        /// Tests if the ingenious computer shoots on the field and misses
        /// </summary>
        [TestMethod()]
        public void ShootAsyncIngeniousTest()
        {
            //Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            var battlefield = new Battlefield(game.Object);
            battlefield.CreateField();
            game.Setup(g => g._BattlefieldPlayer).Returns(battlefield);
            var computerOpponent = new ComputerOpponent(game.Object);
            computerOpponent._YourTurn = true;
            game.Setup(g => g._ComputerDifficulty).Returns(ComputerDifficulty.Genie);
            game.Setup(game => game._ComputerOpponent).Returns(new IngeniousOpponent(game.Object));

            //Act
            computerOpponent.ShootAsync().Wait();
            int x = game.Object._ComputerOpponent._X;
            int y = game.Object._ComputerOpponent._Y;

            //Assert
            Assert.IsTrue(game.Object._BattlefieldPlayer._Board[x, y]._State == SquareState.Miss, $"Squarestate did not change to Miss x:{x} y:{y}");
        }
    }
}