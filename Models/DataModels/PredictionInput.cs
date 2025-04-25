using Core.Domain;

namespace Models.DataModels
{
    public record PredictionInput
    {
        public string MatchId { get; init; }
        public TeamStats HomeStats { get; init; }
        public TeamStats AwayStats { get; init; }
        public WeatherConditions Weather { get; init; }
        public RefereeStats Referee { get; init; }
    }
}
