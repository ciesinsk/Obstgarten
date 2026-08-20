using Obstgarten.Game;

namespace Obstgarten.Strategies
{
    /// <summary>
    /// Chooses two fruits at random from the fruit types that are still available.
    /// Each choice is made independently from the currently available fruit types.
    /// </summary>
    /// <typeparam name="T">The enum type used to represent fruit types.</typeparam>
    public class RandomAvailableFruitStrategy<T> : IChoseFruitsStrategy<T>
        where T : Enum
    {
        private const int NumberOfFruits = 2;

        public IEnumerable<T> ChoseFruits(IGame<T> game)
        {
            var result = new List<T>();
            var fruitsLeft = game.FruitsLeft.ToDictionary(k => k.Key, v => v.Value);

            foreach (var _ in Enumerable.Range(0, NumberOfFruits))
            {
                var availableFruits = fruitsLeft
                    .Where(f => f.Value > 0)
                    .Select(f => f.Key)
                    .ToList();

                if (availableFruits.Count == 0)
                {
                    break;
                }

                var fruit = availableFruits[Random.Shared.Next(availableFruits.Count)];
                result.Add(fruit);
                fruitsLeft[fruit]--;
            }

            return result;
        }
    }
}
