using Core.Domain;
using Core.Enums;

namespace Models.DataModels
{
    public record MatchInput
    {
        public required string MatchId { get; init; }
        public required TeamStats HomeStats { get; init; }
        public required TeamStats AwayStats { get; init; }
        public required WeatherConditions Weather { get; init; }
        public required RefereeStats Referee { get; init; }
        public required League League { get; init; }
        public required DateTime MatchTime { get; init; }
    }
}
