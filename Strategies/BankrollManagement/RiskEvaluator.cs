using Core.Domain;

namespace Strategies.BankrollManagement
{
    public class RiskEvaluator
    {
        private readonly IBankrollStrategy _strategy;

        public RiskEvaluator(IBankrollStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal CalculateRecommendedStake(
            decimal bankroll,
            decimal probability,
            decimal odds,
            decimal maxPercentage = 0.05m)
        {
            decimal stake = _strategy.CalculateStake(bankroll, probability, odds);
            return Math.Min(stake, bankroll * maxPercentage);
        }

        public decimal CalculatePortfolioRisk(IEnumerable<Bet> activeBets, decimal currentBankroll)
        {
            decimal totalExposure = activeBets.Sum(b => b.Stake);
            decimal maxPotentialLoss = activeBets
                .Where(b => !b.IsSettled && !b.IsHedged)
                .Sum(b => b.Stake);

            return maxPotentialLoss / currentBankroll;
        }
    }
}
