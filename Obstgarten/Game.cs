namespace Obstgarten
{
    public class Game<T>: IGame<T> 
        where T : Enum
    {       
        /// <summary>
        /// The number of turns taken so far
        /// </summary>
        public int TurnsTaken{get; private set;}

        public IDictionary<T, int> FruitsLeft{ get; private set;} = new Dictionary<T, int>();
        
        public int RavenPartsLaid {get; private set;}

        public IEnumerable<T> RavenColors {get; private set;}

        public IEnumerable<T> JokerColors {get; private set;}

        public int NumberOfFruitsPerTree => 10;

        public int NumberOfRavenParts => 9;

        public Game() 
        {
            TurnsTaken = 0;
            RavenPartsLaid = 0;
           
            foreach (var fruitType in (T[])  Enum.GetValues(typeof(T)))
            {
                FruitsLeft[fruitType] = NumberOfFruitsPerTree;
            }
        }

        public bool IsGameWon()
        {
            if(HasGameEnded() == false)
            {
                return false;
            }

            if(RavenPartsLaid == NumberOfRavenParts) 
            {
                return false;
            }
            
            return true;
        }

        public bool HasGameEnded()
        {
            if(RavenPartsLaid == NumberOfRavenParts)
            {
                return true;
            }

            if(FruitsLeft.Values.Any(v=>v > 0) == false) 
            {
                return true;
            }

            return false;
        }
    }
}
