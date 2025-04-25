using Microsoft.ML;

namespace Models.PredictiveModels
{
    public class XGBoostModel
    {
        private readonly PredictionEngine<ModelInput, ModelOutput> _predictionEngine;

        public XGBoostModel(string modelPath)
        {
            var mlContext = new MLContext();
            var model = mlContext.Model.Load(modelPath, out _);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
        }

        public ModelOutput Predict(ModelInput input) => _predictionEngine.Predict(input);
    }
}
