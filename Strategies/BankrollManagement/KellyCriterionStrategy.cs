namespace Strategies.BankrollManagement
{
    public class KellyCriterionStrategy : IBankrollStrategy
    {
        public string StrategyName => "Kelly Criterion";

        public decimal CalculateStake(decimal bankroll, decimal probability, decimal odds)
        {
            if (odds <= 1m)
                return 0m;

            decimal q = 1m - probability;
            decimal b = odds - 1m;
            decimal kellyFraction = (b * probability - q) / b;

            // Conservative Kelly (half Kelly)
            return bankroll * kellyFraction * 0.5m;
        }
    }
}
