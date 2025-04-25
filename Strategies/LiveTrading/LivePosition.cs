using Core.Enums;

namespace Strategies.LiveTrading
{
    public record LivePosition
    {
        public string PositionId { get; } = Guid.NewGuid().ToString();
        public string MatchId { get; init; }
        public BetType BetType { get; init; }
        public decimal InitialStake { get; init; }
        public decimal InitialOdds { get; init; }
        public DateTime OpenedAt { get; init; } = DateTime.UtcNow;
        public decimal? HedgeStake { get; set; }
        public decimal? HedgeOdds { get; set; }
        public DateTime? LastHedgeTime { get; set; }
        public bool IsActive => HedgeStake == null || HedgeStake < InitialStake;
    }
}
