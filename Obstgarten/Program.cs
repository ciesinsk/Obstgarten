using Obstgarten.Dices;
using Obstgarten.Game;
using Obstgarten.Strategies;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Obstgarten.");

            const int N = 100000;
            var results = new List<int>();

            // play N games of Obstgarten
            foreach(var i in Enumerable.Range(0, N)) 
            { 
                // play a game of Obstgarten
                IGame<GameParameters.Colors> game = new Game<GameParameters.Colors>
                {
                    Dice = new DefaultDice<GameParameters.Colors>(Guid.NewGuid()),
                    //ChoosingStrategy = new FixedFavouritesStrategy<GameParameters.Colors>(),
                    ChoosingStrategy = new ChoseMostRemainingFruitsStrategy<GameParameters.Colors>(),
                    RavenColors = [GameParameters.Colors.Raven],
                    JokerColors = [GameParameters.Colors.Basket]
                };

                game.InitFruitTree();

                while(game.HasGameEnded() == false)
                {
                    game.TakeTurn();
                }

                if (game.IsGameWon())
                {
                    results.Add(1);
                }
                else
                {
                    results.Add(0);
                }
            }

            Console.WriteLine($"Players won {(double)results.Where(r=>r == 1).Count() / N *100}% of {results.Count} games.");
        }
    }
}