using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.View.Tests
{
    [TestClass()]
    public class BattlefieldTests
    {

        /// <summary>
        /// Tests if the field is created correctly and if Squares can be set to empty
        /// </summary>
        [TestMethod()]
        public void CreatefieldTest()
        {
            // Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            Battlefield battlefield = new Battlefield(game.Object);

            // Act
            battlefield.CreateField();

            // Assert
            for (int i = 0; i < battlefield._Size; i++)
            {
                for (int j = 0; j < battlefield._Size; j++)
                {
                    Assert.IsTrue(battlefield._Board[i, j]._State == Sea.SquareState.Empty, $"Squarestate x: {i} y:{j} is not set to Empty");
                }
            }
            var boardHorizontal = battlefield._Board.GetLength(0);
            var boardVertical = battlefield._Board.GetLength(1);
            Assert.IsTrue(10 == boardHorizontal, $"Horizontalsize is: {boardHorizontal} instead of 10");
            Assert.IsTrue(10 == boardVertical, $"Verticalsize is: {boardVertical} instead od 10");
        }

        /// <summary>
        /// Tests if the ShootAsync method returns false if the square is empty
        /// </summary>
        [TestMethod()]   
        public void ShootAsyncMissTest()
        {
            // Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            Battlefield battlefield = new Battlefield(game.Object);
            battlefield.CreateField();

            // Act
            var result = battlefield.ShootAsync(0, 0).Result;

            // Assert
            Assert.IsTrue(result == false, "ShootAsync returned true instead of false");
        }

        /// <summary>
        /// Tests if the ShootAsync method returns true if the square is a ship
        /// </summary>
        [TestMethod()]
        public void ShootAsyncHitTest()
        {
            // Arrange
            var game = new Mock<GameLogic>();
            game.Setup(g => g._Size).Returns(10);
            Battlefield battlefield = new Battlefield(game.Object);
            battlefield.CreateField();
            Ship ship = new Ship();
            battlefield._Board[0, 0].SetToShipSquare(ship);

            // Act
            var result = battlefield.ShootAsync(0, 0).Result;

            // Assert
            Assert.IsTrue(result == true, "ShootAsync returned false instead of true");
        }

    }
}