using Core.Enums;

namespace Models.DataModels
{
    public record ActualResult
    {
        public string MatchId { get; init; }
        public BetType Outcome { get; init; }
        public int HomeGoals { get; init; }
        public int AwayGoals { get; init; }
        public int YellowCards { get; init; }
        public bool BTTS { get; init; }
    }
}
