using Core.Domain;

namespace Strategies.LiveTrading
{
    public interface IHedgingService
    {
        HedgeDecision EvaluateHedge(LivePosition position, LiveOddsUpdate update);
    }

    public record HedgeDecision
    {
        public bool ShouldHedge { get; init; }
        public decimal HedgeStake { get; init; }
        public decimal HedgeOdds { get; init; }
        public string Reason { get; init; }
    }
}
