using Core.Domain;
using Core.Enums;

namespace Models.Ensemble
{
    public class VotingSystem
    {
        public BetRecommendation? Resolve(
            IEnumerable<ModelPrediction> predictions,
            Dictionary<string, decimal> modelWeights,
            string matchId,
            Dictionary<BetType, decimal> bookmakerOdds)
        {
            if (!predictions.Any() || modelWeights.Count == 0)
                return null;

            var weightedProbs = CalculateWeightedProbabilities(predictions, modelWeights);
            var bestBet = GetBestBet(weightedProbs);

            return CreateBetRecommendation(
                matchId,
                bestBet,
                weightedProbs[bestBet.Key],
                predictions,
                modelWeights,
                bookmakerOdds);
        }

        private static Dictionary<BetType, decimal> CalculateWeightedProbabilities(
            IEnumerable<ModelPrediction> predictions,
            Dictionary<string, decimal> modelWeights)
        {
            var weightedProbs = InitializeProbabilityDictionary();

            foreach (var pred in predictions)
            {
                if (!modelWeights.TryGetValue(pred.ModelName, out var weight))
                    continue;

                AddWeightedProbabilities(weightedProbs, pred.Probabilities, weight);
            }

            return weightedProbs;
        }

        private static Dictionary<BetType, decimal> InitializeProbabilityDictionary()
        {
            return Enum.GetValues<BetType>()
                .ToDictionary(betType => betType, _ => 0m);
        }

        private static void AddWeightedProbabilities(
            Dictionary<BetType, decimal> weightedProbs,
            PredictionProbabilities probabilities,
            decimal weight)
        {
            weightedProbs[BetType.HomeWin] += probabilities.HomeWin * weight;
            weightedProbs[BetType.Draw] += probabilities.Draw * weight;
            weightedProbs[BetType.AwayWin] += probabilities.AwayWin * weight;
            weightedProbs[BetType.Over2_5] += probabilities.Over2_5 * weight;
            weightedProbs[BetType.Under2_5] += probabilities.Under2_5 * weight;
            weightedProbs[BetType.BTTS] += probabilities.BTTS * weight;
            weightedProbs[BetType.BTTS_No] += probabilities.BTTS_No * weight;
        }

        private static KeyValuePair<BetType, decimal> GetBestBet(Dictionary<BetType, decimal> weightedProbs)
        {
            return weightedProbs.MaxBy(kvp => kvp.Value);
        }

        private static BetRecommendation CreateBetRecommendation(
            string matchId,
            KeyValuePair<BetType, decimal> bestBet,
            decimal totalProbability,
            IEnumerable<ModelPrediction> predictions,
            Dictionary<string, decimal> modelWeights,
            Dictionary<BetType, decimal> bookmakerOdds)
        {
            var recommendation = BetRecommendation.CreateSimple(
                matchId: matchId,
                betType: bestBet.Key,
                odds: bookmakerOdds[bestBet.Key],
                probability: totalProbability,
                models: predictions
                    .OrderByDescending(p => modelWeights[p.ModelName])
                    .Select(p => p.ModelName)
            );

            recommendation.CalculateEdge(1m / bookmakerOdds[bestBet.Key]);
            return recommendation;
        }
    }
}
