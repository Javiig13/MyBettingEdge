using Data.Database;
using Models.DataModels;

namespace MyBettingEdge.Data.Database.Repositories
{
    public class RefereeStatsRepository(AppDbContext context) : IRefereeStatsRepository
    {
        public Task<RefereeStats?> GetByRefereeIdAsync(string refereeId)
        {
            return context.RefereeStats
                .FirstOrDefaultAsync(r => r.RefereeId == refereeId);
        }
    }
}
