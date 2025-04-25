using Core.Domain;
using Core.Enums;
using Strategies.Core;

namespace Strategies.ValueBet
{
    public class ValueBetStrategy : IBettingStrategy
    {
        private readonly IEdgeCalculator _edgeCalculator;
        private readonly IMarketFilters _marketFilters;
        private ValueBetConfiguration _config;

        public string StrategyName => "ValueBet";

        public ValueBetStrategy(IEdgeCalculator edgeCalculator, IMarketFilters marketFilters)
        {
            _edgeCalculator = edgeCalculator;
            _marketFilters = marketFilters;
            _config = new ValueBetConfiguration();
        }

        public async Task<StrategyResult> ExecuteAsync(IEnumerable<MatchAnalysis> matches)
        {
            var filteredMatches = _marketFilters.FilterMatches(matches, _config);
            var recommendations = new List<BetRecommendation>();

            foreach (var match in filteredMatches)
            {
                foreach (var betType in _config.AllowedBetTypes)
                {
                    if (_edgeCalculator.HasValue(match, betType, _config.MinEdge))
                    {
                        recommendations.Add(CreateRecommendation(match, betType));
                    }
                }
            }

            return new StrategyResult
            {
                StrategyName = StrategyName,
                Recommendations = recommendations.OrderByDescending(r => r.EdgePercentage),
                ExpectedValue = CalculateExpectedValue(recommendations),
                RiskScore = CalculateRiskScore(recommendations)
            };
        }

        public void UpdateConfiguration(StrategyConfiguration config)
        {
            _config = new ValueBetConfiguration
            {
                MinEdge = config.MinEdge,
                MaxOdds = config.MaxOdds,
                AllowedLeagues = config.AllowedLeagues,
                AllowedBetTypes = config.AllowedBetTypes
            };
        }

        private BetRecommendation CreateRecommendation(MatchAnalysis match, BetType betType)
        {
            var edge = _edgeCalculator.CalculateEdge(match, betType);
            return new BetRecommendation
            {
                MatchId = match.MatchId,
                BetType = betType,
                Odds = match.Odds[betType],
                CalculatedProbability = edge.Probability,
                EdgePercentage = edge.Percentage,
                Confidence = edge.Probability * 100m // Nueva propiedad
            };
        }

        private decimal CalculateExpectedValue(IEnumerable<BetRecommendation> recommendations)
        {
            if (!recommendations.Any()) return 0m;

            return recommendations.Average(r =>
                (r.CalculatedProbability * (r.Odds - 1)) -
                ((1 - r.CalculatedProbability) * 1));
        }

        private decimal CalculateRiskScore(IEnumerable<BetRecommendation> recommendations)
        {
            if (!recommendations.Any()) return 0m;

            decimal maxLoss = recommendations.Sum(r => 1 - r.CalculatedProbability);
            decimal avgEdge = recommendations.Average(r => r.EdgePercentage);

            return maxLoss * (1 - (avgEdge / 100m));
        }
    }
}
