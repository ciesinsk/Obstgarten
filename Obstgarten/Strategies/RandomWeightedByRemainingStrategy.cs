using Obstgarten.Game;

namespace Obstgarten.Strategies
{
    /// <summary>
    /// Chooses two fruits randomly, weighted by the number of fruits remaining.
    /// A fruit type with twice as many remaining fruits is twice as likely to be chosen.
    /// </summary>
    public class RandomWeightedByRemainingStrategy<T> : IChoseFruitsStrategy<T>
        where T : Enum
    {
        private const int NumberOfFruits = 2;

        public IEnumerable<T> ChoseFruits(IGame<T> game)
        {
            var result = new List<T>();
            var fruitsLeft = game.FruitsLeft.ToDictionary(k => k.Key, v => v.Value);

            for (var i = 0; i < NumberOfFruits; i++)
            {
                var available = fruitsLeft.Where(f => f.Value > 0).ToList();
                if (available.Count == 0)
                {
                    break;
                }

                var totalWeight = available.Sum(f => f.Value);
                var choice = Random.Shared.Next(totalWeight);
                var cumulative = 0;

                foreach (var fruit in available)
                {
                    cumulative += fruit.Value;
                    if (choice < cumulative)
                    {
                        result.Add(fruit.Key);
                        fruitsLeft[fruit.Key]--;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
