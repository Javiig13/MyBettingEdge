using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Data.Database.Repositories
{
    public class MatchRepository(AppDbContext context) : IMatchRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Match>> GetUpcomingMatchesAsync(DateTime fromDate)
        {
            return await _context.Matches
                .Where(m => m.StartTime >= fromDate)
                .ToListAsync();
        }

        public async Task<Match?> GetMatchByIdAsync(string matchId)
        {
            return await _context.Matches.FindAsync(matchId);
        }
    }
}