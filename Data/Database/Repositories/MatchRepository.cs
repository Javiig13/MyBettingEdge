using Microsoft.EntityFrameworkCore;
using Core.Domain;

namespace Data.Database.Repositories
{
    public class MatchRepository(AppDbContext context) : IMatchRepository
    {
        public async Task<Match> GetByIdAsync(string matchId)
            => await context.Matches
                .Include(m => m.HomeTeamStats)
                .Include(m => m.AwayTeamStats)
                .FirstOrDefaultAsync(m => m.MatchId == matchId);

        public async Task AddAsync(Match match)
        {
            await context.Matches.AddAsync(match);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Match match)
        {
            context.Matches.Update(match);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Match>> GetLiveMatchesAsync()
            => await context.Matches
                .Where(m => m.Status == MatchStatus.InPlay)
                .ToListAsync();
    }
}