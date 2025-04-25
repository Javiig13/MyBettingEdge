using Core.Enums;

namespace Models.DataModels
{
    public record RefereeStats
    {
        public string RefereeId { get; init; }
        public int MatchesOfficiated { get; init; }
        public decimal AvgYellowCards { get; init; }
        public decimal AvgRedCards { get; init; }
        public decimal HomeWinRate { get; init; }
        public decimal PenaltyFrequency { get; init; } // Por partido
        public Dictionary<League, RefereeLeagueStats> LeagueStats { get; init; } = new();
    }

    public record RefereeLeagueStats
    {
        public int Matches { get; init; }
        public decimal AvgGoals { get; init; }
        public decimal FoulsPerMatch { get; init; }
    }
}
