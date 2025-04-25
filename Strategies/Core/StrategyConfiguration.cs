using Core.Enums;

namespace Strategies.Core
{
    public record StrategyConfiguration
    {
        public decimal MinEdge { get; init; } = 0.05m;
        public decimal MaxOdds { get; init; } = 3.0m;
        public decimal MaxStakePercentage { get; init; } = 0.02m;
        public HashSet<League> AllowedLeagues { get; init; } = new();
        public HashSet<BetType> AllowedBetTypes { get; init; } = new();
    }
}
