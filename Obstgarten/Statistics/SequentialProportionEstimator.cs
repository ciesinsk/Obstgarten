namespace Obstgarten.Statistics
{
    /// <summary>
    /// Provides an anytime-valid confidence sequence for a Bernoulli probability.
    ///
    /// Each call is treated as the next inspection of the running simulation. The
    /// overall error probability is distributed over all inspections using
    /// alpha_k = alpha * 6 / (pi^2 * k^2). Hoeffding's inequality then provides
    /// a confidence interval for the current inspection. By the union bound, all
    /// returned intervals contain the true probability simultaneously with at
    /// least the requested confidence level, so it is valid to stop based on them.
    /// </summary>
    public static class SequentialProportionEstimator
    {
        public readonly record struct ConfidenceInterval(double Estimate, double Lower, double Upper);

        public static ConfidenceInterval GetConfidenceInterval(
            long successes,
            long trials,
            long inspectionNumber,
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

            if (inspectionNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inspectionNumber));
            }

            if (confidenceLevel <= 0.0 || confidenceLevel >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(confidenceLevel));
            }

            var estimate = (double)successes / trials;
            var alpha = 1.0 - confidenceLevel;

            // Sum(k=1..infinity) 6/(pi^2*k^2) = 1, so the total probability
            // of any confidence interval ever missing the true p is <= alpha.
            var alphaAtThisInspection = alpha * 6.0 /
                                        (Math.PI * Math.PI * inspectionNumber * inspectionNumber);

            // Two-sided Hoeffding bound for Bernoulli observations:
            // P(|pHat - p| >= epsilon) <= 2 exp(-2 n epsilon^2).
            var radius = Math.Sqrt(Math.Log(2.0 / alphaAtThisInspection) / (2.0 * trials));

            return new ConfidenceInterval(
                estimate,
                Math.Max(0.0, estimate - radius),
                Math.Min(1.0, estimate + radius));
        }

        /// <summary>
        /// Returns true if the entire confidence interval lies inside the rounding
        /// cell of the displayed percentage. In that case, every probability in
        /// the interval rounds to the same displayed value.
        /// </summary>
        public static bool IsRoundedPercentageStable(
            ConfidenceInterval interval,
            int decimalPlaces)
        {
            if (decimalPlaces < 0 || decimalPlaces > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            }

            var estimatePercent = interval.Estimate * 100.0;
            var lowerPercent = interval.Lower * 100.0;
            var upperPercent = interval.Upper * 100.0;

            var rounded = Math.Round(estimatePercent, decimalPlaces, MidpointRounding.AwayFromZero);
            var halfUnit = 0.5 * Math.Pow(10.0, -decimalPlaces);

            var roundingCellLower = Math.Max(0.0, rounded - halfUnit);
            var roundingCellUpper = Math.Min(100.0, rounded + halfUnit);

            return lowerPercent > roundingCellLower && upperPercent < roundingCellUpper;
        }
    }
}
