namespace Obstgarten.Dices
{
    public interface IDice<T>
        where T : Enum
    {
        int NumberTosses { get; }
        public T NextToss(IGame<T> game);
    }
}
