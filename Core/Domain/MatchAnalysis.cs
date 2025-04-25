using Core.Enums;

namespace Core.Domain
{
    public record MatchAnalysis
    {
        public string MatchId { get; init; }
        public string HomeTeam { get; init; }
        public string AwayTeam { get; init; }
        public League League { get; init; }
        public DateTime StartTime { get; init; }
        public Dictionary<BetType, decimal> Odds { get; init; } = new();
        public Dictionary<BetType, decimal> Probabilities { get; init; } = new();
        public HashSet<BetType> AvailableMarkets { get; init; } = new();
        public ChampionsLeagueStage? Stage { get; init; } // Para Champions League
    }
}
