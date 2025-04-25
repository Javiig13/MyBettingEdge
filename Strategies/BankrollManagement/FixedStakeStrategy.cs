namespace Strategies.BankrollManagement
{
    public class FixedStakeStrategy : IBankrollStrategy
    {
        private readonly decimal _fixedPercentage;

        public FixedStakeStrategy(decimal fixedPercentage = 0.01m)
        {
            _fixedPercentage = fixedPercentage;
        }

        public string StrategyName => $"Fixed Stake ({_fixedPercentage:P0})";

        public decimal CalculateStake(decimal bankroll, decimal probability, decimal odds)
        {
            return bankroll * _fixedPercentage;
        }
    }
}
