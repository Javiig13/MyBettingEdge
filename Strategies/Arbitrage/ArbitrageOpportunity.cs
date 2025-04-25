using Core.Enums;

namespace Strategies.Arbitrage
{
    public record ArbitrageOpportunity
    {
        public string MatchId { get; init; }
        public BetType BetType { get; init; }
        public string BackBookmaker { get; init; }
        public decimal BackOdds { get; init; }
        public string LayBookmaker { get; init; }
        public decimal LayOdds { get; init; }
        public decimal ProfitPercentage { get; init; }
        public DateTime DetectedAt { get; init; }
        public bool IsExpired => (DateTime.UtcNow - DetectedAt).TotalMinutes > 5;
    }
}
