using Obstgarten.Statistics;

namespace UnitTests
{
    [TestClass]
    public class SequentialProportionEstimatorTests
    {
        [TestMethod]
        public void PrecisionForOneDecimalPercentageIsHalfOfLastDisplayedUnit()
        {
            var precision = SequentialProportionEstimator
                .PrecisionForPercentageDecimalPlaces(1);

            Assert.AreEqual(0.0005, precision, 1e-12);
        }

        [TestMethod]
        public void RequiredTrialsForNinetyPercentAndOneDecimalPercentageIsFinite()
        {
            var required = SequentialProportionEstimator.GetRequiredTrials(
                absoluteError: 0.0005,
                confidenceLevel: 0.90);

            Assert.AreEqual(5_991_465L, required);
        }

        [TestMethod]
        public void ConfidenceIntervalContainsEstimate()
        {
            var interval = SequentialProportionEstimator.GetConfidenceInterval(
                successes: 600,
                trials: 1000,
                confidenceLevel: 0.90);

            Assert.IsTrue(interval.Lower <= interval.Estimate);
            Assert.IsTrue(interval.Estimate <= interval.Upper);
            Assert.AreEqual(0.6, interval.Estimate, 1e-12);
        }

        [TestMethod]
        public void ConfidenceIntervalShrinksWithMoreTrials()
        {
            var smallerSample = SequentialProportionEstimator.GetConfidenceInterval(
                successes: 600,
                trials: 1000,
                confidenceLevel: 0.90);

            var largerSample = SequentialProportionEstimator.GetConfidenceInterval(
                successes: 6000,
                trials: 10000,
                confidenceLevel: 0.90);

            Assert.IsTrue(largerSample.Upper - largerSample.Lower <
                          smallerSample.Upper - smallerSample.Lower);
        }
    }
}
