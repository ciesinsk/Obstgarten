using Obstgarten.Statistics;

namespace UnitTests
{
    [TestClass]
    public class SequentialProportionEstimatorTests
    {
        [TestMethod]
        public void ConfidenceIntervalContainsEstimate()
        {
            var interval = SequentialProportionEstimator.GetConfidenceInterval(
                successes: 600,
                trials: 1000,
                inspectionNumber: 1,
                confidenceLevel: 0.90);

            Assert.IsTrue(interval.Lower <= interval.Estimate);
            Assert.IsTrue(interval.Estimate <= interval.Upper);
            Assert.AreEqual(0.6, interval.Estimate, 1e-12);
        }

        [TestMethod]
        public void ConfidenceIntervalShrinksWithMoreTrialsAtSameInspection()
        {
            var smallerSample = SequentialProportionEstimator.GetConfidenceInterval(
                successes: 600,
                trials: 1000,
                inspectionNumber: 1,
                confidenceLevel: 0.90);

            var largerSample = SequentialProportionEstimator.GetConfidenceInterval(
                successes: 6000,
                trials: 10000,
                inspectionNumber: 1,
                confidenceLevel: 0.90);

            Assert.IsTrue(largerSample.Upper - largerSample.Lower <
                          smallerSample.Upper - smallerSample.Lower);
        }

        [TestMethod]
        public void RoundedPercentageIsStableWhenWholeIntervalIsInOneRoundingCell()
        {
            var interval = new SequentialProportionEstimator.ConfidenceInterval(
                Estimate: 0.6342,
                Lower: 0.6338,
                Upper: 0.6344);

            Assert.IsTrue(
                SequentialProportionEstimator.IsRoundedPercentageStable(interval, decimalPlaces: 1));
        }

        [TestMethod]
        public void RoundedPercentageIsNotStableWhenIntervalCrossesRoundingBoundary()
        {
            var interval = new SequentialProportionEstimator.ConfidenceInterval(
                Estimate: 0.6342,
                Lower: 0.6338,
                Upper: 0.6352);

            Assert.IsFalse(
                SequentialProportionEstimator.IsRoundedPercentageStable(interval, decimalPlaces: 1));
        }
    }
}
