using Core.Domain;

namespace Strategies.ValueBet
{
    public interface IMarketFilters
    {
        IEnumerable<MatchAnalysis> FilterMatches(IEnumerable<MatchAnalysis> matches, ValueBetConfiguration config);
    }

    public class MarketFilters : IMarketFilters
    {
        public IEnumerable<MatchAnalysis> FilterMatches(IEnumerable<MatchAnalysis> matches, ValueBetConfiguration config)
        {
            return matches.Where(m =>
                config.AllowedLeagues.Contains(m.League) &&
                m.Odds.Values.All(odds => odds <= config.MaxOdds) &&
                m.Probabilities.Values.All(p => p > 0));
        }
    }
}
