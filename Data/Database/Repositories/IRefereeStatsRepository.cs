using Models.DataModels;

namespace MyBettingEdge.Data.Database.Repositories
{
    public interface IRefereeStatsRepository
    {
        Task<RefereeStats?> GetByRefereeIdAsync(string refereeId);
    }
}
