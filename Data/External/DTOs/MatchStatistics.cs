namespace Data.External.DTOs
{
    public record MatchStatistics(
        string MatchId,
        int HomeGoals,
        int AwayGoals,
        int HomeShots,
        int AwayShots,
        int HomeShotsOnTarget,
        int AwayShotsOnTarget,
        double HomeExpectedGoals,
        double AwayExpectedGoals,
        int HomeCorners,
        int AwayCorners,
        int HomeFouls,
        int AwayFouls,
        Dictionary<string, double> AdvancedMetrics // Ej: posesión, presión
    );
}
