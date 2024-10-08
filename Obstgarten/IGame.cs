namespace Obstgarten
{
    public interface IGame<T>
        where T : Enum
    {
        public IDictionary<T, int> FruitsLeft {get;}

        public bool HasGameEnded();
    }
}
