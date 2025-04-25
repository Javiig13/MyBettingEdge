using Core.Enums;

namespace Core.Domain
{
    public class TeamStats
    {
        public required string TeamId { get; init; }
        public required string TeamName { get; init; }
        public required League MainLeague { get; init; }

        public double CurrentEloRating { get; set; }

        private const int MaxRecentMatches = 10;
        public Queue<MatchPerformance> RecentPerformances { get; } = new(MaxRecentMatches);

        // Promedios móviles
        public double AvgGoalsScored { get; private set; }
        public double AvgGoalsConceded { get; private set; }
        public double AvgxG { get; private set; }
        public double AvgxGA { get; private set; }
        public double AvgCornersFor { get; private set; }
        public double AvgCornersAgainst { get; private set; }
        public double AvgCardsFor { get; private set; }
        public double AvgCardsAgainst { get; private set; }

        public void UpdateStats(MatchPerformance performance)
        {
            if (RecentPerformances.Count == MaxRecentMatches)
                RecentPerformances.Dequeue();

            RecentPerformances.Enqueue(performance);
            RecalculateAverages();
        }

        private void RecalculateAverages()
        {
            AvgGoalsScored = RecentPerformances.Average(p => p.GoalsScored);
            AvgGoalsConceded = RecentPerformances.Average(p => p.GoalsConceded);
            AvgxG = RecentPerformances.Average(p => p.ExpectedGoals);
            AvgxGA = RecentPerformances.Average(p => p.ExpectedGoalsAgainst);
            AvgCornersFor = RecentPerformances.Average(p => p.CornersFor);
            AvgCornersAgainst = RecentPerformances.Average(p => p.CornersAgainst);
            AvgCardsFor = RecentPerformances.Average(p => p.CardsFor);
            AvgCardsAgainst = RecentPerformances.Average(p => p.CardsAgainst);
        }

        public static TeamStats Create(string teamId, string teamName, League league)
        {
            return new TeamStats
            {
                TeamId = teamId,
                TeamName = teamName,
                MainLeague = league
            };
        }
    }

    public record MatchPerformance(
        string MatchId,
        int GoalsScored,
        int GoalsConceded,
        double ExpectedGoals,
        double ExpectedGoalsAgainst,
        int CornersFor,
        int CornersAgainst,
        int CardsFor,
        int CardsAgainst,
        bool IsHomeGame
    );
}
