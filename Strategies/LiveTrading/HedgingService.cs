using Core.Domain;

namespace Strategies.LiveTrading
{
    public class HedgingService : IHedgingService
    {
        private const decimal ProfitThreshold = 0.05m; // 5% profit target
        private const decimal LossThreshold = -0.10m; // 10% max loss

        public HedgeDecision EvaluateHedge(LivePosition position, LiveOddsUpdate update)
        {
            var currentMarketOdds = update.Odds[position.BetType];
            var potentialOutcomes = CalculateOutcomes(position, currentMarketOdds);

            if (potentialOutcomes.MaxProfit >= position.InitialStake * ProfitThreshold)
            {
                return new HedgeDecision
                {
                    ShouldHedge = true,
                    HedgeStake = CalculateOptimalHedgeStake(position, currentMarketOdds),
                    HedgeOdds = currentMarketOdds,
                    Reason = "Profit target reached"
                };
            }

            if (potentialOutcomes.MaxLoss <= position.InitialStake * LossThreshold)
            {
                return new HedgeDecision
                {
                    ShouldHedge = true,
                    HedgeStake = position.InitialStake,
                    HedgeOdds = currentMarketOdds,
                    Reason = "Loss threshold triggered"
                };
            }

            return new HedgeDecision { ShouldHedge = false };
        }

        private (decimal MaxProfit, decimal MaxLoss) CalculateOutcomes(LivePosition position, decimal currentOdds)
        {
            decimal winOutcome = (decimal)(position.InitialStake * (position.InitialOdds - 1) - position.HedgeStake);
            decimal lossOutcome = (decimal)(-position.InitialStake + (position.HedgeStake * (currentOdds - 1)));

            return (Math.Max(winOutcome, lossOutcome), Math.Min(winOutcome, lossOutcome));
        }

        private decimal CalculateOptimalHedgeStake(LivePosition position, decimal currentOdds)
        {
            return position.InitialStake * (position.InitialOdds - 1) / (currentOdds - 1);
        }
    }
}
