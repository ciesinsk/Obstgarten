using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obstgarten.Strategies
{
    public class ChoseMostRemainingFruitsStrategy : IChoseFruitsStrategy<GameParameters.Fruits>
    {
        public IEnumerable<GameParameters.Fruits> ChoseFruits(IGame<GameParameters.Fruits> game)
        {
            var availableFruits = game.FruitsLeft.Select(d=>(fruitType: d.Key, count: d.Value)).OrderByDescending(i => i.count).Where(f=>f.count >0).ToList();

            
            if(availableFruits.First().count == 0) 
            {
                throw new Exception("Most abundant fruit has count zero");
            }

            return availableFruits.Select(f=>f.fruitType);
        }
    }
}
