using Core.Domain;

namespace MyBettingEdge.Data.Database.Repositories
{
    public interface IBetRepository
    {
        Task AddAsync(Bet bet);
        Task<IEnumerable<Bet>> GetAllAsync();
        Task<IEnumerable<Bet>> GetByMatchIdAsync(string matchId);
    }
}
