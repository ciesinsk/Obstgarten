namespace Obstgarten.Statistics
{
    /// <summary>
    /// Helpers for estimating a Bernoulli probability with a requested absolute precision.
    ///
    /// The required sample size is derived from Hoeffding's inequality. Unlike the previous
    /// anytime-valid confidence sequence, the stopping point therefore depends only on the
    /// requested confidence and precision, not on the observed simulation results. This avoids
    /// the severe conservatism of repeated alpha-spending and guarantees finite termination.
    /// </summary>
    public static class SequentialProportionEstimator
    {
        public readonly record struct ConfidenceInterval(double Estimate, double Lower, double Upper);

        /// <summary>
        /// Returns the absolute probability error corresponding to the requested number of
        /// decimal places in a percentage. For example, one decimal place means +/-0.05
        /// percentage points, i.e. +/-0.0005 as a probability.
        /// </summary>
        public static double PrecisionForPercentageDecimalPlaces(int decimalPlaces)
        {
            if (decimalPlaces < 0 || decimalPlaces > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            }

            return 0.5 * Math.Pow(10.0, -decimalPlaces) / 100.0;
        }

        /// <summary>
        /// Returns a conservative sample size that guarantees
        /// P(|pHat - p| &lt;= absoluteError) &gt;= confidenceLevel
        /// for independent Bernoulli trials, using Hoeffding's inequality.
        /// </summary>
        public static long GetRequiredTrials(double absoluteError, double confidenceLevel)
        {
            if (absoluteError <= 0.0 || absoluteError >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(absoluteError));
            }

            if (confidenceLevel <= 0.0 || confidenceLevel >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(confidenceLevel));
            }

            var alpha = 1.0 - confidenceLevel;
            var required = Math.Log(2.0 / alpha) / (2.0 * absoluteError * absoluteError);
            return checked((long)Math.Ceiling(required));
        }

        /// <summary>
        /// Computes the Hoeffding interval at a fixed sample size and confidence level.
        /// </summary>
        public static ConfidenceInterval GetConfidenceInterval(
            long successes,
            long trials,
            double confidenceLevel)
        {
            if (trials <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(trials));
            }

            if (successes < 0 || successes > trials)
            {
                throw new ArgumentOutOfRangeException(nameof(successes));
            }

            if (confidenceLevel <= 0.0 || confidenceLevel >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(confidenceLevel));
            }

            var estimate = (double)successes / trials;
            var alpha = 1.0 - confidenceLevel;
            var radius = Math.Sqrt(Math.Log(2.0 / alpha) / (2.0 * trials));

            return new ConfidenceInterval(
                estimate,
                Math.Max(0.0, estimate - radius),
                Math.Min(1.0, estimate + radius));
        }
    }
}
