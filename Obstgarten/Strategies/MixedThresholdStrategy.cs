using Obstgarten.Game;

namespace Obstgarten.Strategies
{
    /// <summary>
    /// Uses a mixed strategy: while all available fruit types have at least the configured
    /// threshold remaining, it chooses from the most abundant type. As soon as any available
    /// fruit type falls below the threshold, it chooses from the least abundant type instead.
    /// </summary>
    public class MixedThresholdStrategy<T> : IChoseFruitsStrategy<T>
        where T : Enum
    {
        private const int NumberOfFruits = 2;

        public int Threshold { get; }

        public MixedThresholdStrategy(int threshold = 3)
        {
            if (threshold < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be at least 1.");
            }

            Threshold = threshold;
        }

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

                var useLeastRemaining = available.Any(f => f.Value < Threshold);
                var fruit = useLeastRemaining
                    ? available.OrderBy(f => f.Value).First().Key
                    : available.OrderByDescending(f => f.Value).First().Key;

                result.Add(fruit);
                fruitsLeft[fruit]--;
            }

            return result;
        }
    }
}
