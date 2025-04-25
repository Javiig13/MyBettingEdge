using Core.Enums;

namespace Strategies.Arbitrage
{
    public class ArbitrageScanner(IArbitrageCalculator calculator)
    {
        public IEnumerable<ArbitrageOpportunity> FindOpportunities(
            Dictionary<string, Dictionary<BetType, Dictionary<string, decimal>>> allOdds)
        {
            var opportunities = new List<ArbitrageOpportunity>();

            foreach (var (matchId, matchOdds) in allOdds)
            {
                foreach (var betType in matchOdds.Keys)
                {
                    var bookmakerOdds = matchOdds[betType];
                    if (bookmakerOdds.Count < 2) continue;

                    var opportunity = calculator.FindArbitrage(matchId, betType, bookmakerOdds);
                    if (opportunity != null)
                    {
                        opportunities.Add(opportunity);
                    }
                }
            }

            return opportunities
                .OrderByDescending(o => o.ProfitPercentage)
                .ThenBy(o => o.MatchId);
        }
    }
}
