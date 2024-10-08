using Moq;
using Obstgarten;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDefaultDice()
        {
            var game = new Mock<IGame<GameParameters.Fruits>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.Fruits, int>
                {
                    { GameParameters.Fruits.Apples, 1 },
                    { GameParameters.Fruits.Pears, 1 },
                    { GameParameters.Fruits.Plums, 1 },
                    { GameParameters.Fruits.Cherries, 1 },
                }
            );

            var d = new Obstgarten.Dices.DefaultDice<GameParameters.Fruits>();

            var result = Enumerable.Range(0, 10).Select(_ => d.NextToss(game.Object)).ToList();
            
            var reference = new List<GameParameters.Fruits>
            {
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Plums, 
                GameParameters.Fruits.Plums,
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Apples,
                GameParameters.Fruits.Pears,
                GameParameters.Fruits.Plums,
                GameParameters.Fruits.Cherries,
                GameParameters.Fruits.Plums,
                GameParameters.Fruits.Cherries
            };

            Assert.IsTrue( result.SequenceEqual(reference) );   
        }

        [TestMethod]
        public void TestDefaultDice2()
        {
            var game = new Mock<IGame<GameParameters.Fruits>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.Fruits, int>
                {
                    { GameParameters.Fruits.Apples, 1 },
                    { GameParameters.Fruits.Pears, 1 },
                    { GameParameters.Fruits.Plums, 0 },
                    { GameParameters.Fruits.Cherries, 0 },
                }
            );

            var d = new Obstgarten.Dices.DefaultDice<GameParameters.Fruits>();

            var result = Enumerable.Range(0, 10).Select(_ => d.NextToss(game.Object)).ToList();
            
            var reference = new List<GameParameters.Fruits>
            {
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Apples, 
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Apples, 
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Apples, 
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Pears
            };

            Assert.IsTrue( result.SequenceEqual(reference) );   
        }

         [TestMethod]
        public void TestMostAbundantStrategy()
        {
            var game = new Mock<IGame<GameParameters.Fruits>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.Fruits, int>
                {
                    { GameParameters.Fruits.Apples, 1 },
                    { GameParameters.Fruits.Pears, 1 },
                    { GameParameters.Fruits.Plums, 0 },
                    { GameParameters.Fruits.Cherries, 0 },
                }
            );

            var s = new Obstgarten.Strategies.ChoseMostRemainingFruitsStrategy();

            var chosenFruits = s.ChoseFruits(game.Object);

            Assert.IsTrue(chosenFruits.Contains(GameParameters.Fruits.Apples));
            Assert.IsTrue(chosenFruits.Contains(GameParameters.Fruits.Pears));
            Assert.IsTrue(chosenFruits.Count() == 2);
        }

        [TestMethod]
        public void TestMostAbundantStrategy2()
        {
            var game = new Mock<IGame<GameParameters.Fruits>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.Fruits, int>
                {
                    { GameParameters.Fruits.Apples, 1 },
                    { GameParameters.Fruits.Pears, 3 },
                    { GameParameters.Fruits.Plums, 0 },
                    { GameParameters.Fruits.Cherries, 2 },
                }
            );

            var s = new Obstgarten.Strategies.ChoseMostRemainingFruitsStrategy();

            var chosenFruits = s.ChoseFruits(game.Object);
            var reference = new List<GameParameters.Fruits>
            {
                GameParameters.Fruits.Pears, 
                GameParameters.Fruits.Cherries,
                GameParameters.Fruits.Apples
            };

            Assert.IsTrue( chosenFruits.SequenceEqual(reference) );   
        }
    }
}