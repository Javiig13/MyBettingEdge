using Core.Enums;

namespace Strategies.Arbitrage
{
    public class ArbitrageCalculator : IArbitrageCalculator
    {
        private const decimal MinArbitragePercent = 1.0m;

        public ArbitrageOpportunity? FindArbitrage(string matchId, BetType betType, Dictionary<string, decimal> bookmakerOdds)
        {
            if (bookmakerOdds.Count < 2)
                return null;

            var bestBack = bookmakerOdds.OrderByDescending(x => x.Value).First();
            var bestLay = bookmakerOdds.OrderBy(x => x.Value).First();

            if (bestBack.Key == bestLay.Key)
                return null;

            decimal impliedProbability = (1m / bestBack.Value) + (1m / bestLay.Value);
            decimal arbitragePercent = (1m - impliedProbability) * 100m;

            if (arbitragePercent >= MinArbitragePercent)
            {
                return new ArbitrageOpportunity
                {
                    MatchId = matchId,
                    BetType = betType,
                    BackBookmaker = bestBack.Key,
                    BackOdds = bestBack.Value,
                    LayBookmaker = bestLay.Key,
                    LayOdds = bestLay.Value,
                    ProfitPercentage = arbitragePercent,
                    DetectedAt = DateTime.UtcNow
                };
            }

            return null;
        }

        public Dictionary<BetType, decimal> CalculateStakes(ArbitrageOpportunity opportunity, decimal totalStake)
        {
            decimal backStake = totalStake / (1 + (opportunity.BackOdds / opportunity.LayOdds));
            decimal layStake = totalStake - backStake;

            return new Dictionary<BetType, decimal>
            {
                [opportunity.BetType] = backStake,
                [GetOppositeBetType(opportunity.BetType)] = layStake
            };
        }

        private BetType GetOppositeBetType(BetType betType)
        {
            return betType switch
            {
                BetType.HomeWin => BetType.AwayWin,
                BetType.AwayWin => BetType.HomeWin,
                BetType.Over2_5 => BetType.Under2_5,
                BetType.Under2_5 => BetType.Over2_5,
                BetType.BTTS => BetType.BTTS_No,
                BetType.BTTS_No => BetType.BTTS,
                _ => throw new ArgumentException("Unsupported bet type for arbitrage")
            };
        }
    }
}
