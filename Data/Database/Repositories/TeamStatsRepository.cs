using Core.Domain;
using Data.Database;
using Microsoft.EntityFrameworkCore;

namespace MyBettingEdge.Data.Database.Repositories
{
    public class TeamStatsRepository(AppDbContext context) : ITeamStatsRepository
    {
        public Task<TeamStats?> GetByTeamIdAsync(string teamId)
        {
            return context.TeamStats
                .FirstOrDefaultAsync(t => t.TeamId == teamId);
        }
    }
}
