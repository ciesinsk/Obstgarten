namespace Obstgarten
{
    public interface IGame<T>
        where T : Enum
    {
        public IDictionary<T, int> FruitsLeft {get;}
        
        int NumberOfFruitsPerTree {get;}

        int NumberOfRavenParts {get;}

        int RavenPartsLaid { get; }

        IEnumerable<T> RavenColors { get; }

        IEnumerable<T> JokerColors { get; }

        public bool HasGameEnded();
        bool IsGameWon();
    }
}
