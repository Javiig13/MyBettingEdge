using Core.Domain;
using Core.Enums;

namespace Data.External.DTOs
{
    public record LiveMatch(
        string MatchId,
        string HomeTeam,
        string AwayTeam,
        int HomeScore,
        int AwayScore,
        int Minute,
        MatchStatus Status,
        Dictionary<BetType, decimal> CurrentOdds,
        DateTime LastUpdated
    );
}
