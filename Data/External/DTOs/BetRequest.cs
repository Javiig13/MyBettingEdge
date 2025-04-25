using Core.Enums;

namespace Data.External.DTOs
{
    public record BetRequest(
        string MatchId,
        BetType BetType,
        decimal Stake,
        decimal Odds);
}
