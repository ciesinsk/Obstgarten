namespace Obstgarten.Strategies
{
    public interface IChoseFruitsStrategy<T>
        where T : Enum
    {
        public IEnumerable<T> ChoseFruits(IGame<T> game);
    }
}
