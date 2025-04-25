using Core.Domain;

namespace Data.Database.Repositories
{
    public interface IMatchRepository
    {
        Task<Match> GetByIdAsync(string matchId);
        Task AddAsync(Match match);
        Task UpdateAsync(Match match);
        Task<IEnumerable<Match>> GetLiveMatchesAsync();
    }
}