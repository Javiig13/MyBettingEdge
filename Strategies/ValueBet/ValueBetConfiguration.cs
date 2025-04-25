using Strategies.Core;

namespace Strategies.ValueBet
{
    public record ValueBetConfiguration : StrategyConfiguration
    {
        public decimal MinProbability { get; init; } = 0.10m;
        public int MinHistoricalMatches { get; init; } = 10;
    }
}
