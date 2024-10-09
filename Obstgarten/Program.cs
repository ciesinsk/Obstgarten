using Obstgarten.Dices;
using Obstgarten.Game;
using Obstgarten.Statistics;
using Obstgarten.Strategies;


namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Obstgarten.");
            
            const int N = 100000;
            var results = new List<ResultRecord<GameParameters.DefaultColors>>();

            // play N games of Obstgarten
            foreach(var i in Enumerable.Range(0, N)) 
            { 
                // play a game of Obstgarten
                IGame<GameParameters.DefaultColors> game = new Game<GameParameters.DefaultColors>
                {
                    Dice = new DefaultDice<GameParameters.DefaultColors>(Guid.NewGuid()),
                    //ChoosingStrategy = new FixedFavouritesStrategy<GameParameters.Colors>(),
                    ChoosingStrategy = new ChoseMostRemainingFruitsStrategy<GameParameters.DefaultColors>(),
                    RavenColors = [GameParameters.DefaultColors.Raven],
                    JokerColors = [GameParameters.DefaultColors.Basket],
                    NumberOfRavenParts = 9
                };

                game.InitFruitTree();

                while(game.HasGameEnded() == false)
                {
                    game.TakeTurn();
                }

                if(game is IGameResult<GameParameters.DefaultColors> gameResult) 
                {
                    results.Add(new ResultRecord<GameParameters.DefaultColors>(gameResult));
                }
            }

            if(results.Count == 0) 
            {
                Console.WriteLine("no results");
            }

            Console.WriteLine($"Players won {(double)results.Where(r=>r.PlayersWon).Count() / N *100}% of {results.Count} games.");
        }
    }
}