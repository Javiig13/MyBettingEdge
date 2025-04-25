using Core.Domain;

namespace Strategies.Core
{
    public record StrategyResult
    {
        public string StrategyName { get; init; }
        public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
        public IEnumerable<BetRecommendation> Recommendations { get; init; } = [];
        public decimal ExpectedValue { get; init; }
        public decimal RiskScore { get; init; }
    }
}
