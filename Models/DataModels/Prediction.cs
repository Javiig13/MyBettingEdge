using Core.Enums;

namespace Models.DataModels
{
    public record Prediction
    {
        public string MatchId { get; init; }
        public BetType BetType { get; init; }
        public decimal Odds { get; init; }
        public decimal Stake { get; init; }
        public bool? Success { get; init; } // Null si no se ha resuelto
        public DateTime PredictedAt { get; init; }
    }
}
