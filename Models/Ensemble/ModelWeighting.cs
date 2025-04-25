namespace Models.Ensemble
{
    public class ModelWeighting
    {
        private readonly Dictionary<string, decimal> _weights = new()
        {
            ["PoissonModel"] = 0.25m,
            ["EloRatingModel"] = 0.30m,
            ["XGBoostModel"] = 0.45m
        };

        public void UpdateWeight(string modelName, decimal newWeight)
        {
            if (_weights.ContainsKey(modelName))
            {
                _weights[modelName] = Math.Clamp(newWeight, 0.1m, 0.8m);
                NormalizeWeights();
            }
        }

        private void NormalizeWeights()
        {
            decimal total = _weights.Values.Sum();
            foreach (var key in _weights.Keys.ToList())
            {
                _weights[key] /= total;
            }
        }

        public decimal GetWeight(string modelName) =>
            _weights.GetValueOrDefault(modelName, 0m);
    }
}
