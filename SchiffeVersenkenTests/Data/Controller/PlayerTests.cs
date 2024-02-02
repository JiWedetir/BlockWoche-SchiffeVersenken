using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;
using SchiffeVersenken.Data.View;

namespace SchiffeVersenken.Data.Controller.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        /// <summary>
        /// Tests if the ships can be set on the board
        /// </summary>
        [TestMethod()]
        public void CheckShipsTrueTest()
        {
            // Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            Player player = new Player(game.Object);
            List<ShipDetails> ships = new List<ShipDetails>();
            ships.Add(new ShipDetails() { PositionX = 0, PositionY = 0, Size = 5, Orientation = Orientation.Horizontal });
            ships.Add(new ShipDetails() { PositionX = 0, PositionY = 2, Size = 4, Orientation = Orientation.Vertical });
            ships.Add(new ShipDetails() { PositionX = 4, PositionY = 2, Size = 3, Orientation = Orientation.Horizontal });
            ships.Add(new ShipDetails() { PositionX = 6, PositionY = 3, Size = 2, Orientation = Orientation.Vertical });

            // Act
            var result = player.CheckShips(ships);

            // Assert
            Assert.IsTrue(result == true, "Ships could not be set although they were correct");
        }

        /// <summary>
        /// Tests if the ships can not be set on the board if they are not set correctly
        /// </summary>
        [TestMethod()]
        public void CheckShipsFalseTest()
        {
            // Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            Player player = new Player(game.Object);
            List<ShipDetails> ships = new List<ShipDetails>();
            ships.Add(new ShipDetails() { PositionX = 0, PositionY = 0, Size = 5, Orientation = Orientation.Horizontal });
            ships.Add(new ShipDetails() { PositionX = 2, PositionY = 1, Size = 4, Orientation = Orientation.Vertical });

            // Act
            var result = player.CheckShips(ships);

            // Assert
            Assert.IsTrue(result == false, "Ships culd be set although they were not correct");
        }

        /// <summary>
        /// Tests if the ships can be set on the board and if the board is set correctly
        /// </summary>
        [TestMethod()]
        public void SetShipsTest()
        {
            // Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            Player player = new Player(game.Object);
            List<ShipDetails> ships = new List<ShipDetails>();
            ships.Add(new ShipDetails() { PositionX = 0, PositionY = 0, Size = 5, Orientation = Orientation.Horizontal });
            ships.Add(new ShipDetails() { PositionX = 3, PositionY = 2, Size = 4, Orientation = Orientation.Vertical });
            var battlefield = new Battlefield(game.Object);
            battlefield.CreateField();
            game.Setup(g => g._BattlefieldPlayer).Returns(battlefield);

            // Act
            var result = player.SetShips(ships);

            // Assert
            Assert.IsTrue(result == true, "Ships couldent be set although they were correct");
            Assert.IsTrue(battlefield._Board[0, 0]._State == SquareState.Ship, $"Board 0/0 is: {battlefield._Board[0, 0]._State} instead of Ship");
            Assert.IsTrue(battlefield._Board[1, 0]._State == SquareState.Ship, $"Board 1/0 is: {battlefield._Board[1, 0]._State} instead of Ship");
            Assert.IsTrue(battlefield._Board[2, 0]._State == SquareState.Ship, $"Board 2/0 is: {battlefield._Board[2, 0]._State} instead of Ship");
            Assert.IsTrue(battlefield._Board[3, 0]._State == SquareState.Ship, $"Board 3/0 is: {battlefield._Board[3, 0]._State} instead of Ship");
            Assert.IsTrue(battlefield._Board[4, 0]._State == SquareState.Ship, $"Board 4/0 is: {battlefield._Board[4, 0]._State} instead of Ship");
            Assert.IsTrue(battlefield._Board[3, 2]._State == SquareState.Ship, $"Board 3/2 is: {battlefield._Board[3, 2]._State} instead of Ship");
            Assert.IsTrue(battlefield._Board[3, 3]._State == SquareState.Ship, $"Board 3/3 is: {battlefield._Board[3, 3]._State} instead of Ship");
            Assert.IsTrue(battlefield._Board[3, 4]._State == SquareState.Ship, $"Board 3/4 is: {battlefield._Board[3, 4]._State} instead of Ship");
            Assert.IsTrue(battlefield._Board[3, 5]._State == SquareState.Ship, $"Board 3/5 is: {battlefield._Board[3, 5]._State} instead of Ship");
        }

        /// <summary>
        /// Tests if a Player shoots on a square of the opponent
        /// </summary>
        [TestMethod()]
        public void ShootTest()
        {
            // Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            Player player = new Player(game.Object);
            player._YourTurn = true;
            var battlefield = new Battlefield(game.Object);
            battlefield.CreateField();
            game.Setup(g => g._BattlefieldOpponent).Returns(battlefield);

            // Act
            player.Shoot(0, 0);

            // Assert
            Assert.IsTrue(game.Object._BattlefieldOpponent._Board[0, 0]._State == SquareState.Miss, $"Board 0/0 is: {game.Object._BattlefieldOpponent._Board[0, 0]._State} instead of Miss");
        }
    }
}