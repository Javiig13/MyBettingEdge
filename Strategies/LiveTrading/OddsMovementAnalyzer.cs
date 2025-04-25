using Core.Domain;
using Core.Enums;

namespace Strategies.LiveTrading
{
    public class OddsMovementAnalyzer
    {
        private readonly Dictionary<string, List<OddsSnapshot>> _oddsHistory = new();

        public void RecordOdds(LiveOddsUpdate update)
        {
            if (!_oddsHistory.ContainsKey(update.MatchId))
            {
                _oddsHistory[update.MatchId] = new List<OddsSnapshot>();
            }

            _oddsHistory[update.MatchId].Add(new OddsSnapshot
            {
                Timestamp = DateTime.UtcNow,
                Odds = update.Odds
            });
        }

        public TrendAnalysis AnalyzeTrend(string matchId, BetType betType, int lookbackMinutes = 5)
        {
            if (!_oddsHistory.ContainsKey(matchId) || _oddsHistory[matchId].Count < 2)
                return TrendAnalysis.Neutral;

            var recentOdds = _oddsHistory[matchId]
                .Where(x => x.Timestamp > DateTime.UtcNow.AddMinutes(-lookbackMinutes))
                .Select(x => x.Odds[betType])
                .ToList();

            if (recentOdds.Count < 2)
                return TrendAnalysis.Neutral;

            decimal change = recentOdds.Last() - recentOdds.First();
            decimal percentageChange = change / recentOdds.First();

            return percentageChange switch
            {
                < -0.05m => TrendAnalysis.StrongDown,
                < -0.02m => TrendAnalysis.ModerateDown,
                > 0.05m => TrendAnalysis.StrongUp,
                > 0.02m => TrendAnalysis.ModerateUp,
                _ => TrendAnalysis.Neutral
            };
        }
    }

    public enum TrendAnalysis
    {
        StrongUp,
        ModerateUp,
        Neutral,
        ModerateDown,
        StrongDown
    }

    public record OddsSnapshot
    {
        public DateTime Timestamp { get; init; }
        public Dictionary<BetType, decimal> Odds { get; init; }
    }
}
