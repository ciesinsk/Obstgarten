using Obstgarten.Dices;
using Obstgarten.Strategies;

namespace Obstgarten
{
    public class Game<T>: IGame<T> 
        where T : Enum
    {       
        /// <summary>
        /// The number of turns taken so far
        /// </summary>
        public int TurnsTaken{get; private set;}

        /// <summary>
        /// A dictionary that keeps track of the fruits left on the tree
        /// </summary>
        public IDictionary<T, int> FruitsLeft{ get; init;} 
        
        /// <summary>
        /// A counter that keeps track of the number of Raven tikes laid out yet
        /// </summary>
        public int RavenPartsLaid {get; private set;}

        /// <summary>
        /// The colors that represent the raven (in the unaltered game this is <see cref="GameParameters.Colors.Raven"/>).
        /// </summary>
        public IEnumerable<T> RavenColors {get; init;} = Enumerable.Empty<T>();

        /// <summary>
        /// The color where you are allowed to chose two different fruits in one turn. 
        /// </summary>
        public IEnumerable<T> JokerColors {get; init;} = Enumerable.Empty<T>();

        /// <summary>
        /// The default starting value for the number of each fruit on the tree
        /// </summary>
        public int NumberOfFruitsPerTree{get; init;} = 10;

        /// <summary>
        /// The number of Raven parts there are.
        /// </summary>
        public int NumberOfRavenParts {get; init;} = 9;

        /// <summary>
        /// The dice that is used
        /// </summary>
        public IDice<T> Dice {get; init; } = new DefaultDice<T>();

        public IChoseFruitsStrategy<T> ChoosingStrategy {get; init;} = new ChoseMostRemainingFruitsStrategy<T>();

        /// <summary>
        /// The class that definis the gameplay
        /// </summary>
        public Game() 
        {
            TurnsTaken = 0;
            RavenPartsLaid = 0;
           
            FruitsLeft = new Dictionary<T, int>();
            foreach (var fruitType in (T[])  Enum.GetValues(typeof(T)))
            {
                FruitsLeft[fruitType] = NumberOfFruitsPerTree;
            }
        }

        public void TakeTurn()
        {
            var nextToss = Dice.NextToss(this);

            if (RavenColors.Contains(nextToss))
            {
                RavenPartsLaid++;
                return;
            }

            if (JokerColors.Contains(nextToss))
            {
                var selection = ChoosingStrategy.ChoseFruits(this);
                foreach(var fruit in selection)
                {

                }
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
