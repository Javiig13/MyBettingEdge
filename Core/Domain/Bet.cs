using Core.Enums;

namespace Core.Domain
{
    public record Bet
    {
        public string BetId { get; init; } = Guid.NewGuid().ToString();
        public string MatchId { get; init; }
        public BetType BetType { get; init; }
        public decimal Stake { get; init; }
        public decimal Odds { get; init; }
        public DateTime PlacedAt { get; init; } = DateTime.UtcNow;
        public bool IsSettled { get; init; } = false;
        public bool IsHedged { get; init; } = false;
        public BetStatus Status { get; init; } = BetStatus.Pending;
    }

    public enum BetStatus
    {
        Pending,
        Won,
        Lost,
        Voided
    }
}
