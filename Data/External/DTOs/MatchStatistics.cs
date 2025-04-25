namespace Data.External.DTOs
{
    public record class MatchStatistics(
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
    )
    {
        // Propiedades calculadas

        public int TotalGoals => HomeGoals + AwayGoals;
        public int TotalShots => HomeShots + AwayShots;
        public int TotalShotsOnTarget => HomeShotsOnTarget + AwayShotsOnTarget;
        public int TotalCorners => HomeCorners + AwayCorners;
        public int TotalFouls => HomeFouls + AwayFouls;
        public double ExpectedGoalsTotal => HomeExpectedGoals + AwayExpectedGoals;

        // Validación simple de consistencia
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(MatchId) &&
            HomeExpectedGoals >= 0 && AwayExpectedGoals >= 0 &&
            HomeGoals >= 0 && AwayGoals >= 0 &&
            HomeShots >= 0 && AwayShots >= 0 &&
            AdvancedMetrics is not null;
    }
}
