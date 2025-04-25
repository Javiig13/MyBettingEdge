using Core.Domain;
using Models.Ensemble;

namespace Models.PredictiveModels
{
    public class EloRatingModel
    {
        private const decimal BaseOverProbability = 0.65m;
        private const decimal BaseUnderProbability = 0.35m;
        private const decimal LineDefault = 2.5m;

        public static PredictionProbabilities CalculateProbabilities(TeamStats homeStats, TeamStats awayStats)
        {
            decimal eloDiff = (decimal)awayStats.CurrentEloRating - (decimal)homeStats.CurrentEloRating + 100m;
            decimal homeWinProb = 1m / (1m + (decimal)Math.Pow(10, (double)(eloDiff / 400m)));

            decimal overUnderProb = CalculateOverUnderProbability(homeStats, awayStats, LineDefault);
            decimal expectedGoals = CalculateExpectedGoals(homeStats, awayStats);

            return new PredictionProbabilities
            {
                HomeWin = homeWinProb,
                Draw = 0.12m,
                AwayWin = 1m - homeWinProb - 0.12m,
                Over2_5 = overUnderProb,
                Under2_5 = 1m - overUnderProb,
                ExpectedGoals = expectedGoals,
                ExpectedCorners = CalculateExpectedCorners(homeStats, awayStats),
                ExpectedCards = CalculateExpectedCards(homeStats, awayStats),
                BTTS = CalculateBttsProbability(homeStats, awayStats),
                BTTS_No = 1m - CalculateBttsProbability(homeStats, awayStats)
            };
        }

        private static decimal CalculateOverUnderProbability(TeamStats home, TeamStats away, decimal line)
        {
            decimal homeExpected = (decimal)(home.AvgGoalsScored * away.AvgGoalsConceded);
            decimal awayExpected = (decimal)(away.AvgGoalsScored * home.AvgGoalsConceded);
            return homeExpected + awayExpected > line ? BaseOverProbability : BaseUnderProbability;
        }

        private static decimal CalculateExpectedGoals(TeamStats home, TeamStats away)
        {
            return (decimal)((home.AvgGoalsScored + away.AvgGoalsConceded) / 2.0 +
                            (away.AvgGoalsScored + home.AvgGoalsConceded) / 2.0);
        }

        private static decimal CalculateExpectedCorners(TeamStats home, TeamStats away)
        {
            return (decimal)((home.AvgCornersFor + away.AvgCornersAgainst) / 2.0);
        }

        private static decimal CalculateExpectedCards(TeamStats home, TeamStats away)
        {
            return (decimal)((home.AvgCardsFor + away.AvgCardsAgainst) / 2.0);
        }

        private static decimal CalculateBttsProbability(TeamStats home, TeamStats away)
        {
            decimal homeScoringProb = (decimal)(home.AvgGoalsScored / (away.AvgGoalsConceded + 0.1));
            decimal awayScoringProb = (decimal)(away.AvgGoalsScored / (home.AvgGoalsConceded + 0.1));
            return Math.Clamp((homeScoringProb + awayScoringProb) / 2m, 0.1m, 0.9m);
        }
    }
}