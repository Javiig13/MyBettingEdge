namespace Data.External.DTOs
{
    public record TeamStatsResponse(
        string TeamId,
        string TeamName,
        int MatchesPlayed,
        double AvgGoalsScored,
        double AvgGoalsConceded,
        double WinRate,
        double CleanSheetRate,
        Dictionary<string, double> PerformanceByLeague
    );
}
