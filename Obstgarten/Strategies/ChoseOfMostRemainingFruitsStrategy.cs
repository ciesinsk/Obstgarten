using Obstgarten.Game;

namespace Obstgarten.Strategies
{
     /// <summary>
    /// A chosing strategy that choses two of the most abundant fruits.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChoseOfMostRemainingFruitsStrategy<T> : IChoseFruitsStrategy<T>
        where T: Enum
    {
        private const int NumberOfFruits = 2;


        public IEnumerable<T> ChoseFruits(IGame<T> game)
        {
            List<T> result = new List<T>();
            var copyOfFruitsLeft = game.FruitsLeft.ToDictionary(k=>k.Key, v=>v.Value);

            foreach(var i in Enumerable.Range(0, NumberOfFruits))
            {
                var fruitList = copyOfFruitsLeft.Select(d=>(fruitType: d.Key, count: d.Value)).OrderByDescending(i => i.count).Where(f=>f.count >0).Select(f=>f.fruitType);
                if(fruitList.Any()) 
                {
                    var fruit = fruitList.First();
                    result.Add(fruit);
                    copyOfFruitsLeft[fruit]--;
                }
            }

            return result;            
        }
    }
}
