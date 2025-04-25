using Core.Domain;

namespace Strategies.Core
{
    public interface IBettingStrategy
    {
        string StrategyName { get; }
        Task<StrategyResult> ExecuteAsync(IEnumerable<MatchAnalysis> matches);
        void UpdateConfiguration(StrategyConfiguration config);
    }
}
