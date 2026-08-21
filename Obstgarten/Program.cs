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

            var absoluteError = SequentialProportionEstimator
                .PrecisionForPercentageDecimalPlaces(PercentageDecimalPlaces);
            var requiredGames = SequentialProportionEstimator
                .GetRequiredTrials(absoluteError, ConfidenceLevel);

            long gamesPlayed = 0;
            long gamesWon = 0;

            var parDegree = Environment.ProcessorCount;
            var parOpt = new ParallelOptions { MaxDegreeOfParallelism = parDegree };

            Console.WriteLine(
                $"Target: +/-{absoluteError * 100:F{PercentageDecimalPlaces + 1}} percentage points " +
                $"with {ConfidenceLevel:P0} confidence.");
            Console.WriteLine(
                $"Required games (Hoeffding bound): {requiredGames:N0}. " +
                $"Degree of parallelism is {parDegree}.");

            while (gamesPlayed < requiredGames)
            {
                var currentBatchSize = (int)Math.Min(BatchSize, requiredGames - gamesPlayed);
                long winsInBatch = 0;

                Parallel.For(
                    0,
                    currentBatchSize,
                    parOpt,
                    () => 0L,
                    (i, state, localWins) => localWins + (PlayGame() ? 1 : 0),
                    localWins => Interlocked.Add(ref winsInBatch, localWins));

                gamesPlayed += currentBatchSize;
                gamesWon += winsInBatch;

                var estimate = (double)gamesWon / gamesPlayed;
                Console.Write(
                    $"\rGames: {gamesPlayed:N0}/{requiredGames:N0}, " +
                    $"win rate: {estimate * 100:F3}%");
            }

            Console.WriteLine();

            var interval = SequentialProportionEstimator.GetConfidenceInterval(
                gamesWon,
                gamesPlayed,
                ConfidenceLevel);

            var displayedWinRate = (interval.Estimate * 100.0).ToString($"F{PercentageDecimalPlaces}");
            Console.WriteLine(
                $"Players won {displayedWinRate}% of {gamesPlayed:N0} games.");
            Console.WriteLine(
                $"With {ConfidenceLevel:P0} confidence, the absolute estimation error is at most " +
                $"{absoluteError * 100:F{PercentageDecimalPlaces + 1}} percentage points " +
                $"(Hoeffding interval: {interval.Lower * 100:F4}% to {interval.Upper * 100:F4}%).");
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
