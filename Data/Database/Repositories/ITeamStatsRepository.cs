using Core.Domain;

namespace MyBettingEdge.Data.Database.Repositories
{
    public interface ITeamStatsRepository
    {
        Task<TeamStats?> GetByTeamIdAsync(string teamId);
    }
}
