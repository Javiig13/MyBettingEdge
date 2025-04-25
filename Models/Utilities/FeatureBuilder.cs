using Core.Domain;
using Models.PredictiveModels;

namespace Models.Utilities
{
    public static class FeatureBuilder
    {
        public static ModelInput BuildFeatures(Match match, TeamStats homeStats, TeamStats awayStats)
        {
            return new ModelInput
            {
                HomeElo = (float)homeStats.CurrentEloRating,
                AwayElo = (float)awayStats.CurrentEloRating,
                HomeFormLast5 = CalculateForm(homeStats.RecentMatches),
                AwayFormLast5 = CalculateForm(awayStats.RecentMatches),
                HomeAvgGoalsScored = (float)homeStats.AvgGoalsScored,
                AwayAvgGoalsScored = (float)awayStats.AvgGoalsScored,
                HomeAvgGoalsConceded = (float)homeStats.AvgGoalsConceded,
                AwayAvgGoalsConceded = (float)awayStats.AvgGoalsConceded,
                // ... otras features
                IsDerby = IsDerbyMatch(match.HomeTeam, match.AwayTeam)
            };
        }

        private static float CalculateForm(IEnumerable<MatchPerformance> performances)
        {
            return performances?.Sum(p => p.GoalsScored - p.GoalsConceded) ?? 0f;
        }

        private static bool IsDerbyMatch(string homeTeam, string awayTeam)
        {
            var derbies = new Dictionary<string, string[]>
            {
                ["Real Madrid"] = ["Atletico Madrid", "Barcelona"],
                ["Barcelona"] = ["Espanyol", "Real Madrid"]
            };

            return derbies.GetValueOrDefault(homeTeam, []).Contains(awayTeam);
        }
    }
}
