using Models.DataModels;

namespace Models.PredictiveModels
{
    public class HybridModel
    {
        private readonly PoissonModel _poisson;
        private readonly EloRatingModel _elo;

        public HybridModel()
        {
            _poisson = new PoissonModel();
            _elo = new EloRatingModel();
        }

        public PredictionOutput Predict(MatchInput input)
        {
            var poissonProbs = _poisson.CalculateProbabilities(input.HomeStats, input.AwayStats);
            var eloProbs = EloRatingModel.CalculateProbabilities(input.HomeStats, input.AwayStats);

            return new PredictionOutput
            {
                HomeWinProbability = (poissonProbs.HomeWin * 0.4m) + (eloProbs.HomeWin * 0.6m),
                DrawProbability = (poissonProbs.Draw * 0.3m) + (eloProbs.Draw * 0.7m),
                AwayWinProbability = (poissonProbs.AwayWin * 0.4m) + (eloProbs.AwayWin * 0.6m),
                Over2_5Probability = (poissonProbs.Over2_5 * 0.5m) + (eloProbs.Over2_5 * 0.5m),
                Under2_5Probability = (poissonProbs.Under2_5 * 0.5m) + (eloProbs.Under2_5 * 0.5m),
                BTTSProbability = (poissonProbs.BTTS * 0.5m) + (eloProbs.BTTS * 0.5m),
                BTTS_NoProbability = (poissonProbs.BTTS_No * 0.5m) + (eloProbs.BTTS_No * 0.5m),
                ExpectedGoals = (poissonProbs.ExpectedGoals * 0.5m) + (eloProbs.ExpectedGoals * 0.5m),
                ExpectedCorners = (poissonProbs.ExpectedCorners * 0.5m) + (eloProbs.ExpectedCorners * 0.5m),
                ExpectedCards = (poissonProbs.ExpectedCards * 0.5m) + (eloProbs.ExpectedCards * 0.5m)
            };
        }
    }
}
