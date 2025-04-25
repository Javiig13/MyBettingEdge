using Core.Domain;
using Microsoft.Extensions.Logging;

namespace Strategies.LiveTrading
{
    public class LiveTradingEngine
    {
        private readonly IHedgingService _hedgingService;
        private readonly List<LivePosition> _openPositions = new();
        private readonly ILogger<LiveTradingEngine> _logger;

        public LiveTradingEngine(IHedgingService hedgingService, ILogger<LiveTradingEngine> logger)
        {
            _hedgingService = hedgingService;
            _logger = logger;
        }

        public void ProcessLiveOdds(LiveOddsUpdate update)
        {
            try
            {
                var relevantPositions = _openPositions
                    .Where(p => p.MatchId == update.MatchId && p.IsActive)
                    .ToList();

                foreach (var position in relevantPositions)
                {
                    var hedgeDecision = _hedgingService.EvaluateHedge(position, update);

                    if (hedgeDecision.ShouldHedge)
                    {
                        ExecuteHedge(position, hedgeDecision);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing live odds for match {MatchId}", update.MatchId);
            }
        }

        private void ExecuteHedge(LivePosition position, HedgeDecision decision)
        {
            // Implementación de lógica de ejecución real
            position.HedgeStake = decision.HedgeStake;
            position.HedgeOdds = decision.HedgeOdds;
            position.LastHedgeTime = DateTime.UtcNow;

            _logger.LogInformation("Hedged position {PositionId} with stake {Stake} at odds {Odds}",
                position.PositionId, decision.HedgeStake, decision.HedgeOdds);
        }

        public void AddPosition(LivePosition position)
        {
            _openPositions.Add(position);
            _logger.LogInformation("Added new position {PositionId} for match {MatchId}",
                position.PositionId, position.MatchId);
        }
    }
}
