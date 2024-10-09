using Moq;
using Obstgarten.Game;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDefaultDice()
        {
            var game = new Mock<IGame<GameParameters.DefaultColors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.DefaultColors, int>
                {
                    { GameParameters.DefaultColors.Apples, 1 },
                    { GameParameters.DefaultColors.Pears, 1 },
                    { GameParameters.DefaultColors.Plums, 1 },
                    { GameParameters.DefaultColors.Cherries, 1 },
                }
            );

            var d = new Obstgarten.Dices.DefaultDice<GameParameters.DefaultColors>();

            var result = Enumerable.Range(0, 10).Select(_ => d.NextToss(game.Object)).ToList();
            
            var reference = new List<GameParameters.DefaultColors>
            {
                GameParameters.DefaultColors.Pears, 
                GameParameters.DefaultColors.Pears, 
                GameParameters.DefaultColors.Pears,
                GameParameters.DefaultColors.Cherries, 
                GameParameters.DefaultColors.Cherries,
                GameParameters.DefaultColors.Plums,
                GameParameters.DefaultColors.Apples,
                GameParameters.DefaultColors.Plums,
                GameParameters.DefaultColors.Apples,
                GameParameters.DefaultColors.Cherries
            };

            Assert.IsTrue( result.SequenceEqual(reference) );   
        }

        [TestMethod]
        public void TestDefaultDice2()
        {
            var game = new Mock<IGame<GameParameters.DefaultColors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.DefaultColors, int>
                {
                    { GameParameters.DefaultColors.Apples, 1 },
                    { GameParameters.DefaultColors.Pears, 1 },
                    { GameParameters.DefaultColors.Plums, 0 },
                    { GameParameters.DefaultColors.Cherries, 0 },
                }
            );

            var d = new Obstgarten.Dices.DefaultDice<GameParameters.DefaultColors>();

            var result = Enumerable.Range(0, 10).Select(_ => d.NextToss(game.Object)).ToList();
            
            var reference = new List<GameParameters.DefaultColors>
            {
                GameParameters.DefaultColors.Pears, 
                GameParameters.DefaultColors.Pears,                 
                GameParameters.DefaultColors.Pears, 
                GameParameters.DefaultColors.Apples, 
                GameParameters.DefaultColors.Apples, 
                GameParameters.DefaultColors.Apples, 
                GameParameters.DefaultColors.Pears, 
                GameParameters.DefaultColors.Pears, 
                GameParameters.DefaultColors.Pears, 
                GameParameters.DefaultColors.Pears
            };

            Assert.IsTrue( result.SequenceEqual(reference) );   
        }

         [TestMethod]
        public void TestMostAbundantStrategy()
        {
            var game = new Mock<IGame<GameParameters.DefaultColors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.DefaultColors, int>
                {
                    { GameParameters.DefaultColors.Apples, 1 },
                    { GameParameters.DefaultColors.Pears, 1 },
                    { GameParameters.DefaultColors.Plums, 0 },
                    { GameParameters.DefaultColors.Cherries, 0 },
                }
            );

            var s = new Obstgarten.Strategies.ChoseMostRemainingFruitsStrategy<GameParameters.DefaultColors>();

            var chosenFruits = s.ChoseFruits(game.Object);

            Assert.IsTrue(chosenFruits.Contains(GameParameters.DefaultColors.Apples));
            Assert.IsTrue(chosenFruits.Contains(GameParameters.DefaultColors.Pears));
            Assert.IsTrue(chosenFruits.Count() == 2);
        }

        [TestMethod]
        public void TestMostAbundantStrategy2()
        {
            var game = new Mock<IGame<GameParameters.DefaultColors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.DefaultColors, int>
                {
                    { GameParameters.DefaultColors.Apples, 1 },
                    { GameParameters.DefaultColors.Pears, 3 },
                    { GameParameters.DefaultColors.Plums, 0 },
                    { GameParameters.DefaultColors.Cherries, 2 },
                }
            );

            var s = new Obstgarten.Strategies.ChoseMostRemainingFruitsStrategy<GameParameters.DefaultColors>();

            var chosenFruits = s.ChoseFruits(game.Object);

            Assert.IsTrue(chosenFruits.Contains(GameParameters.DefaultColors.Cherries));
            Assert.IsTrue(chosenFruits.Contains(GameParameters.DefaultColors.Pears));
            Assert.IsTrue(chosenFruits.Count() == 2);            
        }

        [TestMethod]
        public void TestMostAbundantStrategy3()
        {
            var game = new Mock<IGame<GameParameters.DefaultColors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.DefaultColors, int>
                {
                    { GameParameters.DefaultColors.Apples, 1 },
                    { GameParameters.DefaultColors.Pears, 0 },
                    { GameParameters.DefaultColors.Plums, 0 },
                    { GameParameters.DefaultColors.Cherries, 0 },
                }
            );

            var s = new Obstgarten.Strategies.ChoseMostRemainingFruitsStrategy<GameParameters.DefaultColors>();

            var chosenFruits = s.ChoseFruits(game.Object);

            Assert.IsTrue(chosenFruits.Contains(GameParameters.DefaultColors.Apples));
            Assert.IsTrue(chosenFruits.Count() == 1);            
        }

        [TestMethod]
        public void TestFixedFavStrategy3()
        {
            var game = new Mock<IGame<GameParameters.DefaultColors>>();

            game.Setup(x => x.HasGameEnded()).Returns(false);
            game.SetupGet(x=> x.FruitsLeft).Returns(
                new Dictionary<GameParameters.DefaultColors, int>
                {
                    { GameParameters.DefaultColors.Apples, 1 },
                    { GameParameters.DefaultColors.Pears, 1 },
                    { GameParameters.DefaultColors.Plums, 1 },
                    { GameParameters.DefaultColors.Cherries, 1 },
                }
            );

            var s = new Obstgarten.Strategies.FixedFavouritesStrategy<GameParameters.DefaultColors>();

            var chosenFruits = s.ChoseFruits(game.Object);

            Assert.IsTrue(chosenFruits.Contains(GameParameters.DefaultColors.Apples));
            Assert.IsTrue(chosenFruits.Contains(GameParameters.DefaultColors.Cherries));
            Assert.IsTrue(chosenFruits.Count() == 2);            
        }
    }
}