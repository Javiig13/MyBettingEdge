using Core.Domain;
using Core.Enums;

namespace Strategies.ValueBet
{
    public interface IEdgeCalculator
    {
        EdgeInfo CalculateEdge(MatchAnalysis match, BetType betType);
        bool HasValue(MatchAnalysis match, BetType betType, decimal minEdge);
        decimal CalculateStake(EdgeInfo edge, decimal maxStakePercentage);
    }

    public class EdgeCalculator : IEdgeCalculator
    {
        public EdgeInfo CalculateEdge(MatchAnalysis match, BetType betType)
        {
            decimal impliedProbability = 1m / match.Odds[betType];
            decimal edge = match.Probabilities[betType] - impliedProbability;

            return new EdgeInfo
            {
                Probability = match.Probabilities[betType],
                Percentage = edge * 100,
                Odds = match.Odds[betType]
            };
        }

        public bool HasValue(MatchAnalysis match, BetType betType, decimal minEdge)
        {
            var edge = CalculateEdge(match, betType);
            return edge.Percentage >= minEdge * 100;
        }

        public decimal CalculateStake(EdgeInfo edge, decimal maxStakePercentage)
        {
            decimal kellyFraction = edge.Probability - ((1m - edge.Probability) / (edge.Odds - 1m));
            return Math.Min(kellyFraction, maxStakePercentage);
        }
    }

    public record EdgeInfo
    {
        public decimal Probability { get; init; }
        public decimal Percentage { get; init; }
        public decimal Odds { get; init; }
    }
}
