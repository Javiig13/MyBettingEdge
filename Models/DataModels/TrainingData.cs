using Core.Enums;

namespace Models.DataModels
{
    public record TrainingData
    {
        public List<MatchRecord> HistoricalMatches { get; init; } = [];
        public List<TeamForm> TeamForms { get; init; } = [];
        public List<InjuryRecord> Injuries { get; init; } = [];
    }

    public record MatchRecord(
        string MatchId,
        string HomeTeam,
        string AwayTeam,
        int HomeGoals,
        int AwayGoals,
        decimal HomeOdd,
        decimal AwayOdd,
        decimal DrawOdd,
        League League,
        DateTime Date);

    public record TeamForm(
        string TeamId,
        int MatchesPlayed,
        double AvgGoalsScored,
        double AvgGoalsConceded,
        double XgDiffLast5);

    public record InjuryRecord(
        string TeamId,
        int KeyPlayersMissing,
        bool IsDefensiveLineAffected);
}
