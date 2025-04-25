using Core.Domain;
using Microsoft.ML;
using Models.Ensemble;
using Models.Utilities;

namespace Models.PredictiveModels
{
    public class XGBoostModel
    {
        private readonly PredictionEngine<ModelInput, ModelOutput>? _predictionEngine;

        public XGBoostModel() { }

        public XGBoostModel(string modelPath)
        {
            var mlContext = new MLContext();
            var model = mlContext.Model.Load(modelPath, out _);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
        }

        public ModelOutput Predict(ModelInput input)
        {
            return _predictionEngine?.Predict(input) ?? new ModelOutput();
        }

        public Task<PredictionProbabilities> PredictAsync(Match match, TeamStats homeStats, TeamStats awayStats)
        {
            var modelInput = FeatureBuilder.BuildFeatures(match, homeStats, awayStats);
            var output = Predict(modelInput);

            var probs = new PredictionProbabilities
            {
                HomeWin = output.HomeWinProbability,
                Draw = output.DrawProbability,
                AwayWin = output.AwayWinProbability,
                Over2_5 = output.Over2_5Probability,
                Under2_5 = output.Under2_5Probability,
                BTTS = output.BTTSProbability,
                BTTS_No = output.BTTS_NoProbability,
                ExpectedGoals = output.ExpectedGoals,
                ExpectedCorners = output.ExpectedCorners,
                ExpectedCards = output.ExpectedCards
            };

            return Task.FromResult(probs);
        }
    }
}
