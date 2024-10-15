using Obstgarten.Dices;
using Obstgarten.Game;
using Obstgarten.Statistics;
using Obstgarten.Strategies;
using System.Collections.Concurrent;


namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Obstgarten.");
            
            const int N = 2 << 15;
            var results = new ConcurrentBag<ResultRecord<GameParameters.DefaultColors>>();

            var parDegree = Environment.ProcessorCount;
            var parOpt = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

            Console.WriteLine($"Simulating {N} games, degree of parallelism is {parDegree}.");

            Parallel.For(0, N, parOpt, result => 
            {
                // play a game of Obstgarten
                IGame<GameParameters.DefaultColors> game = new Game<GameParameters.DefaultColors>
                {
                    Dice = new DefaultDice<GameParameters.DefaultColors>(Guid.NewGuid()),
                    //ChoosingStrategy = new FixedFavouritesStrategy<GameParameters.Colors>(),
                    ChoosingStrategy = new ChoseOfMostRemainingFruitsStrategy<GameParameters.DefaultColors>(),
                    RavenColors = [GameParameters.DefaultColors.Raven],
                    JokerColors = [GameParameters.DefaultColors.Basket],
                    NumberOfRavenParts = 9
                };

                game.InitFruitTree();

                while (game.HasGameEnded() == false)
                {
                    game.TakeTurn();
                }

                if (game is IGameResult<GameParameters.DefaultColors> gameResult)
                {
                    results.Add(new ResultRecord<GameParameters.DefaultColors>(gameResult));
                }
            });

            if(results.Count == 0) 
            {
                Console.WriteLine("no results");
            }

            Console.WriteLine($"Players won {(double)results.Where(r=>r.PlayersWon).Count() / N *100}% of {results.Count} games.");
        }
    }
}