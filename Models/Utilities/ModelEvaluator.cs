using Models.DataModels;

namespace Models.Utilities
{
    public static class ModelEvaluator
    {
        public static EvaluationResult Evaluate(
            IEnumerable<Prediction> predictions,
            IEnumerable<ActualResult> actuals)
        {
            var joinedData = predictions.Join(actuals,
                p => p.MatchId,
                a => a.MatchId,
                (p, a) => new { Prediction = p, Actual = a });

            int total = joinedData.Count();
            int correct = joinedData.Count(x => IsPredictionCorrect(x.Prediction, x.Actual));
            int totalPositive = joinedData.Count(x => x.Actual.Outcome == x.Prediction.BetType);

            double accuracy = total > 0 ? correct / (double)total : 0;
            double precision = totalPositive > 0 ?
                joinedData.Count(x => x.Actual.Outcome == x.Prediction.BetType && x.Prediction.Success == true) / (double)totalPositive : 0;

            decimal roi = total > 0 ?
                joinedData.Sum(x => x.Prediction.Success == true ?
                    (x.Prediction.Stake * (x.Prediction.Odds - 1)) :
                    -x.Prediction.Stake) / joinedData.Sum(x => x.Prediction.Stake) : 0;

            return new EvaluationResult
            {
                Accuracy = accuracy,
                Precision = precision,
                ROI = (double)roi
            };
        }

        private static bool IsPredictionCorrect(Prediction p, ActualResult a)
        {
            if (p.Success.HasValue)
                return p.Success.Value && p.BetType == a.Outcome;

            return p.BetType == a.Outcome;
        }
    }

    public record EvaluationResult
    {
        public double Accuracy { get; init; }
        public double Precision { get; init; }
        public double ROI { get; init; }
    }
}
