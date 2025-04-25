using Core.Enums;

namespace Strategies.Arbitrage
{
    public interface IArbitrageCalculator
    {
        ArbitrageOpportunity? FindArbitrage(string matchId, BetType betType, Dictionary<string, decimal> bookmakerOdds);
        Dictionary<BetType, decimal> CalculateStakes(ArbitrageOpportunity opportunity, decimal totalStake);
    }
}
