namespace Obstgarten.Strategies
{
    public class ChoseMostRemainingFruitsStrategy<T> : IChoseFruitsStrategy<T>
        where T: Enum
    {
        private const int NumberOdFruits = 2;


        public IEnumerable<T> ChoseFruits(IGame<T> game)
        {
            var availableFruits = game.FruitsLeft.Select(d=>(fruitType: d.Key, count: d.Value)).OrderByDescending(i => i.count).Where(f=>f.count >0).Select(f => f.fruitType). ToList();                                  
            return availableFruits.Take(NumberOdFruits);
        }
    }
}
