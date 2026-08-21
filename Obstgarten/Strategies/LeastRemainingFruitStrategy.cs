using Obstgarten.Game;

namespace Obstgarten.Strategies
{
    /// <summary>
    /// Chooses two fruits, always preferring the fruit type with the smallest
    /// positive number of fruits remaining.
    /// </summary>
    public class LeastRemainingFruitStrategy<T> : IChoseFruitsStrategy<T>
        where T : Enum
    {
        private const int NumberOfFruits = 2;

        public IEnumerable<T> ChoseFruits(IGame<T> game)
        {
            var result = new List<T>();
            var fruitsLeft = game.FruitsLeft.ToDictionary(k => k.Key, v => v.Value);

            for (var i = 0; i < NumberOfFruits; i++)
            {
                var available = fruitsLeft
                    .Where(f => f.Value > 0)
                    .OrderBy(f => f.Value)
                    .ToList();

                if (available.Count == 0)
                {
                    break;
                }

                var fruit = available[0].Key;
                result.Add(fruit);
                fruitsLeft[fruit]--;
            }

            return result;
        }
    }
}
