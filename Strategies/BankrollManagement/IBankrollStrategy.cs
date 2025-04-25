namespace Strategies.BankrollManagement
{
    public interface IBankrollStrategy
    {
        decimal CalculateStake(decimal bankroll, decimal probability, decimal odds);
        string StrategyName { get; }
    }
}
