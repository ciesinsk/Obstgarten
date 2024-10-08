using Obstgarten.Dices;
using Obstgarten.Strategies;
using System.Text;

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
        public IDictionary<T, int> FruitsLeft{ get; private set;} 
        
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
        public T LastToss { get; private set; }

        /// <summary>
        /// The class that defines the gameplay
        /// </summary>
        public Game() 
        {
            TurnsTaken = 0;
            RavenPartsLaid = 0;                      
        }

        public void InitFruitTree()
        {
            FruitsLeft = new Dictionary<T, int>();
            foreach (var fruitType in ((T[])Enum.GetValues(typeof(T))).Where(f=>JokerColors.Contains(f) == false && RavenColors.Contains(f) == false ))
            {
                FruitsLeft[fruitType] = NumberOfFruitsPerTree;
            }
        }

        public void TakeTurn()
        {
            if(HasGameEnded() == true)
            {
                return;
            }

            var toss = Dice.NextToss(this);

            TurnsTaken++;
            LastToss = toss;

            if (RavenColors.Contains(toss))
            {
                RavenPartsLaid++;
                return;
            }

            if (JokerColors.Contains(toss))
            {
                var selection = ChoosingStrategy.ChoseFruits(this);
                foreach(var fruit in selection)
                {
                    FruitsLeft[fruit] --;
                }

                return;
            }    
            
            FruitsLeft[toss]--;
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

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Turns taken: {TurnsTaken}");
            sb.AppendLine($"Dices thrown: {Dice.NumberTosses}");
            if(TurnsTaken > 0) 
            {
                sb.AppendLine($"Last Toss: {LastToss}");
            }
            sb.AppendLine($"Raven tiles laid: {RavenPartsLaid}");
            foreach(var fruit in FruitsLeft.Keys) 
            {
                sb.AppendLine($"Number of fruit {fruit} left: {FruitsLeft[fruit]}");
            }

            return sb.ToString();
        }
    }
}
