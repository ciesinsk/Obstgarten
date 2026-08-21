using Obstgarten.Dices;
using Obstgarten.Game;
using Obstgarten.Statistics;
using Obstgarten.Strategies;

namespace MyApp
{
    internal class Program
    {
        private const double ConfidenceLevel = 0.90;
        private const int PercentageDecimalPlaces = 1;
        private const int BatchSize = 4096;

        static void Main(string[] args)
        {
            Console.WriteLine("Obstgarten.");

            long gamesPlayed = 0;
            long gamesWon = 0;
            long inspectionNumber = 0;

            var parDegree = Environment.ProcessorCount;
            var parOpt = new ParallelOptions { MaxDegreeOfParallelism = parDegree };

            Console.WriteLine(
                $"Simulating until the win percentage is stable to {PercentageDecimalPlaces} decimal place(s) " +
                $"with {ConfidenceLevel:P0} confidence. Degree of parallelism is {parDegree}.");

            SequentialProportionEstimator.ConfidenceInterval interval;

            do
            {
                long winsInBatch = 0;

                Parallel.For(
                    0,
                    BatchSize,
                    parOpt,
                    () => 0L,
                    (i, state, localWins) => localWins + (PlayGame() ? 1 : 0),
                    localWins => Interlocked.Add(ref winsInBatch, localWins));

                gamesPlayed += BatchSize;
                gamesWon += winsInBatch;
                inspectionNumber++;

                interval = SequentialProportionEstimator.GetConfidenceInterval(
                    gamesWon,
                    gamesPlayed,
                    inspectionNumber,
                    ConfidenceLevel);

                Console.Write(
                    $"\rGames: {gamesPlayed:N0}, " +
                    $"win rate: {interval.Estimate * 100:F3}%, " +
                    $"confidence sequence: [{interval.Lower * 100:F3}%, {interval.Upper * 100:F3}%]");
            }
            while (!SequentialProportionEstimator.IsRoundedPercentageStable(
                       interval,
                       PercentageDecimalPlaces));

            Console.WriteLine();
            Console.WriteLine(
                $"Players won {interval.Estimate * 100:F{PercentageDecimalPlaces}}% of {gamesPlayed:N0} games.");
            Console.WriteLine(
                $"With {ConfidenceLevel:P0} confidence, the true win probability lies between " +
                $"{interval.Lower * 100:F4}% and {interval.Upper * 100:F4}%, and every value in that interval " +
                $"rounds to the same {PercentageDecimalPlaces}-decimal-place percentage.");
        }

        private static bool PlayGame()
        {
            IGame<GameParameters.DefaultColors> game = new Game<GameParameters.DefaultColors>
            {
                Dice = new DefaultDice<GameParameters.DefaultColors>(Guid.NewGuid()),
                ChoosingStrategy = new ChoseOfMostRemainingFruitsStrategy<GameParameters.DefaultColors>(),
                RavenColors = [GameParameters.DefaultColors.Raven],
                JokerColors = [GameParameters.DefaultColors.Basket],
                NumberOfRavenParts = 9
            };

            game.InitFruitTree();

            while (!game.HasGameEnded())
            {
                game.TakeTurn();
            }

            return game is IGameResult<GameParameters.DefaultColors> gameResult && gameResult.PlayersWon;
        }
    }
}
