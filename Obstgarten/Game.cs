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
        
        public int RavenParts {get; private set;}

        public Game() 
        {
            TurnsTaken = 0;
            RavenParts = 0;

            foreach (var fruitType in (T[])  Enum.GetValues(typeof(T)))
            {
                FruitsLeft[fruitType] = GameParameters.NumberFruitsItems;
            }
        }

        public bool IsGameWon()
        {
            if(HasGameEnded() == false)
            {
                return false;
            }

            if(RavenParts == GameParameters.NumberRavenTiles) 
            {
                return false;
            }
            
            return true;
        }

        public bool HasGameEnded()
        {
            if(RavenParts == 9)
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
