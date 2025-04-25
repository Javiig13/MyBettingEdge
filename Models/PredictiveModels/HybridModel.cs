using Core.Domain;
using Models.DataModels;
using Models.Ensemble;

namespace Models.PredictiveModels
{
    public class HybridModel
    {
        private readonly PoissonModel _poisson = new();
        private readonly EloRatingModel _elo = new();
        private readonly XGBoostModel _xgboost;

        public HybridModel(string? xgboostModelPath = null)
        {
            _xgboost = xgboostModelPath is not null
                ? new XGBoostModel(xgboostModelPath)
                : new XGBoostModel(); // Dummy mode
        }

        public async Task<List<ModelPrediction>> PredictAllAsync(Match match, MatchInput input)
        {
            var predictions = new List<ModelPrediction>();

            // Poisson
            var poissonProbs = _poisson.CalculateProbabilities(input.HomeStats, input.AwayStats);
            predictions.Add(new ModelPrediction
            {
                ModelName = "Poisson",
                Probabilities = poissonProbs
            });

            // Elo
            var eloProbs = _elo.CalculateProbabilities(input.HomeStats, input.AwayStats);
            predictions.Add(new ModelPrediction
            {
                ModelName = "Elo",
                Probabilities = eloProbs
            });

            // XGBoost
            var xgbProbs = await _xgboost.PredictAsync(match, input.HomeStats, input.AwayStats);
            predictions.Add(new ModelPrediction
            {
                ModelName = "XGBoost",
                Probabilities = xgbProbs
            });

            return predictions;
        }
    }
}
