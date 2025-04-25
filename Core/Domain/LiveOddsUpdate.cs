using Core.Enums;

namespace Core.Domain
{
    public record LiveOddsUpdate
    {
        public string MatchId { get; init; }
        public Dictionary<BetType, decimal> Odds { get; init; } = new();
        public int Minute { get; init; }
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
        public Score CurrentScore { get; init; }
    }

    public record Score
    {
        public int Home { get; init; }
        public int Away { get; init; }
    }
}
