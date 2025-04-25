using Core.Domain;
using Core.Enums;
using Data.External.DTOs;

namespace Data.External
{
    public interface IBettingApiClient
    {
        Task<MatchOdds> GetLatestOddsAsync(string matchId);
        Task<IEnumerable<LiveMatch>> GetLiveMatchesAsync(League league);
        Task<TeamStats> GetTeamStatsAsync(string teamId);
        Task<bool> PlaceBetAsync(BetRequest request);
    }
}
