﻿using Obstgarten.Game;

namespace Obstgarten.Strategies
{
    /// <summary>
    /// A chosing strategy that choses two of the most abundant fruits.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChoseTwoODifferentfMostRemainingFruitsStrategy<T> : IChoseFruitsStrategy<T>
        where T: Enum
    {
        private const int NumberOfFruits = 2;


        public IEnumerable<T> ChoseFruits(IGame<T> game)
        {
            // choses two different fruits of the most abundant types
            var availableFruits = game.FruitsLeft.Select(d=>(fruitType: d.Key, count: d.Value)).OrderByDescending(i => i.count).Where(f=>f.count >0).Select(f => f.fruitType). ToList();                                  
            return availableFruits.Take(NumberOfFruits);
        }
    }
}
