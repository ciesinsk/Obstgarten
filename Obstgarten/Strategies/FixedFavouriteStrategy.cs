using Obstgarten.Game;

namespace Obstgarten.Strategies
{
    /// <summary>
    /// Chooses fruits according to a fixed preference order.
    /// The same favourite may be chosen twice if at least two are still available.
    /// </summary>
    public class FixedFavouriteStrategy<T> : IChoseFruitsStrategy<T>
        where T : Enum
    {
        private const int NumberOfFruits = 2;
        private readonly IReadOnlyList<T> favourites;

        public FixedFavouriteStrategy(IEnumerable<T> favourites)
        {
            this.favourites = favourites?.Distinct().ToList()
                ?? throw new ArgumentNullException(nameof(favourites));

            if (this.favourites.Count == 0)
            {
                throw new ArgumentException("At least one favourite fruit must be supplied.", nameof(favourites));
            }
        }

        public IEnumerable<T> ChoseFruits(IGame<T> game)
        {
            var result = new List<T>();
            var fruitsLeft = game.FruitsLeft.ToDictionary(k => k.Key, v => v.Value);

            for (var i = 0; i < NumberOfFruits; i++)
            {
                var selected = false;

                foreach (var favourite in favourites)
                {
                    if (fruitsLeft.TryGetValue(favourite, out var count) && count > 0)
                    {
                        result.Add(favourite);
                        fruitsLeft[favourite]--;
                        selected = true;
                        break;
                    }
                }

                if (!selected)
                {
                    break;
                }
            }

            return result;
        }
    }
}
