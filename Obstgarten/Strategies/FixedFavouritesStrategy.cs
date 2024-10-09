namespace Obstgarten.Strategies
{
    /// <summary>
    /// A chosing strategy that choses based on the most favourite fruits regardless of whether there are still many on the tree left or not. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
  public class FixedFavouritesStrategy<T> : IChoseFruitsStrategy<T>
        where T: Enum
    {
        private const int NumberOfFruits = 2;


        public IEnumerable<T> ChoseFruits(IGame<T> game)
        {
            // just order by _some_ criterium OBDA
            var availableFruits = game.FruitsLeft.Select(d=>(fruitType: d.Key, count: d.Value)).OrderByDescending(i => i.fruitType.ToString()).Where(f=>f.count >0).Select(f => f.fruitType). ToList();                                  
            return availableFruits.Take(NumberOfFruits);
        }
    }
}
