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
            var game = new Mock<IGame<GameParameters.Colors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.Colors, int>
                {
                    { GameParameters.Colors.Apples, 1 },
                    { GameParameters.Colors.Pears, 1 },
                    { GameParameters.Colors.Plums, 1 },
                    { GameParameters.Colors.Cherries, 1 },
                }
            );

            var d = new Obstgarten.Dices.DefaultDice<GameParameters.Colors>();

            var result = Enumerable.Range(0, 10).Select(_ => d.NextToss(game.Object)).ToList();
            
            var reference = new List<GameParameters.Colors>
            {
                GameParameters.Colors.Pears, 
                GameParameters.Colors.Pears, 
                GameParameters.Colors.Pears,
                GameParameters.Colors.Cherries, 
                GameParameters.Colors.Cherries,
                GameParameters.Colors.Plums,
                GameParameters.Colors.Apples,
                GameParameters.Colors.Plums,
                GameParameters.Colors.Apples,
                GameParameters.Colors.Cherries
            };

            Assert.IsTrue( result.SequenceEqual(reference) );   
        }

        [TestMethod]
        public void TestDefaultDice2()
        {
            var game = new Mock<IGame<GameParameters.Colors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.Colors, int>
                {
                    { GameParameters.Colors.Apples, 1 },
                    { GameParameters.Colors.Pears, 1 },
                    { GameParameters.Colors.Plums, 0 },
                    { GameParameters.Colors.Cherries, 0 },
                }
            );

            var d = new Obstgarten.Dices.DefaultDice<GameParameters.Colors>();

            var result = Enumerable.Range(0, 10).Select(_ => d.NextToss(game.Object)).ToList();
            
            var reference = new List<GameParameters.Colors>
            {
                GameParameters.Colors.Pears, 
                GameParameters.Colors.Pears,                 
                GameParameters.Colors.Pears, 
                GameParameters.Colors.Apples, 
                GameParameters.Colors.Apples, 
                GameParameters.Colors.Apples, 
                GameParameters.Colors.Pears, 
                GameParameters.Colors.Pears, 
                GameParameters.Colors.Pears, 
                GameParameters.Colors.Pears
            };

            Assert.IsTrue( result.SequenceEqual(reference) );   
        }

         [TestMethod]
        public void TestMostAbundantStrategy()
        {
            var game = new Mock<IGame<GameParameters.Colors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.Colors, int>
                {
                    { GameParameters.Colors.Apples, 1 },
                    { GameParameters.Colors.Pears, 1 },
                    { GameParameters.Colors.Plums, 0 },
                    { GameParameters.Colors.Cherries, 0 },
                }
            );

            var s = new Obstgarten.Strategies.ChoseMostRemainingFruitsStrategy();

            var chosenFruits = s.ChoseFruits(game.Object);

            Assert.IsTrue(chosenFruits.Contains(GameParameters.Colors.Apples));
            Assert.IsTrue(chosenFruits.Contains(GameParameters.Colors.Pears));
            Assert.IsTrue(chosenFruits.Count() == 2);
        }

        [TestMethod]
        public void TestMostAbundantStrategy2()
        {
            var game = new Mock<IGame<GameParameters.Colors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.Colors, int>
                {
                    { GameParameters.Colors.Apples, 1 },
                    { GameParameters.Colors.Pears, 3 },
                    { GameParameters.Colors.Plums, 0 },
                    { GameParameters.Colors.Cherries, 2 },
                }
            );

            var s = new Obstgarten.Strategies.ChoseMostRemainingFruitsStrategy();

            var chosenFruits = s.ChoseFruits(game.Object);
            var reference = new List<GameParameters.Colors>
            {
                GameParameters.Colors.Pears, 
                GameParameters.Colors.Cherries,
                GameParameters.Colors.Apples
            };

            Assert.IsTrue( chosenFruits.SequenceEqual(reference) );   
        }
    }
}