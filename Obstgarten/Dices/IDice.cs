namespace Obstgarten.Dices
{
    public interface IDice<T>
        where T : Enum
    {
        public T NextToss(IGame<T> game);
    }
}
