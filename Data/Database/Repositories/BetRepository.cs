using Core.Domain;
using Data.Database;
using Microsoft.EntityFrameworkCore;

namespace MyBettingEdge.Data.Database.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly AppDbContext _context;

        public BetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Bet bet)
        {
            await _context.Bets.AddAsync(bet);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Bet>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Bet>>(_context.Bets.AsEnumerable());
        }

        public async Task<IEnumerable<Bet>> GetByMatchIdAsync(string matchId)
        {
            return await _context.Bets
                .Where(b => b.MatchId == matchId)
                .ToListAsync();
        }
    }
}
