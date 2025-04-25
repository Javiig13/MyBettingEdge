using Core.Domain;

namespace Data.Database.Repositories
{
    public interface IMatchRepository
    {
        Task<List<Match>> GetUpcomingMatchesAsync(DateTime fromDate);
        Task<Match?> GetMatchByIdAsync(string matchId);
    }
}