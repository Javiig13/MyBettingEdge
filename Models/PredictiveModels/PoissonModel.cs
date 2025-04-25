using Core.Domain;
using Models.Ensemble;

namespace Models.PredictiveModels
{
    public class PoissonModel
    {
        public PredictionProbabilities CalculateProbabilities(TeamStats homeStats, TeamStats awayStats)
        {
            double homeLambda = homeStats.AvgGoalsScored * awayStats.AvgGoalsConceded;
            double awayLambda = awayStats.AvgGoalsScored * homeStats.AvgGoalsConceded;

            return new PredictionProbabilities
            {
                HomeWin = CalculateWinProbability(homeLambda, awayLambda),
                Draw = CalculateDrawProbability(homeLambda, awayLambda),
                AwayWin = CalculateWinProbability(awayLambda, homeLambda),
                Over2_5 = CalculateOverProbability(homeLambda, awayLambda),
                Under2_5 = 1m - CalculateOverProbability(homeLambda, awayLambda),
                ExpectedGoals = (decimal)(homeLambda + awayLambda),
                ExpectedCorners = CalculateExpectedCorners(homeStats, awayStats),
                ExpectedCards = CalculateExpectedCards(homeStats, awayStats)
            };
        }

        private static decimal CalculateExpectedCorners(TeamStats home, TeamStats away)
        {
            return (decimal)(home.AvgCornersFor + away.AvgCornersAgainst) / 2m;
        }

        private static decimal CalculateExpectedCards(TeamStats home, TeamStats away)
        {
            return (decimal)(home.AvgCardsFor + away.AvgCardsAgainst);
        }

        private static decimal CalculateWinProbability(double teamLambda, double opponentLambda)
        {
            return (decimal)(Math.Exp(-teamLambda) * Math.Pow(teamLambda, 1) / Factorial(1));
        }

        private static decimal CalculateDrawProbability(double homeLambda, double awayLambda)
        {
            double expTerm = Math.Exp(-(homeLambda + awayLambda));
            double powTerm = Math.Pow(homeLambda + awayLambda, 1);
            double factorial = Factorial(1);

            return (decimal)(expTerm * powTerm / factorial);
        }

        private static decimal CalculateOverProbability(double homeLambda, double awayLambda, double line = 2.5)
        {
            // Implementación simplificada
            double total = homeLambda + awayLambda;
            return total > line ? 0.7m : 0.3m;
        }

        private static int Factorial(int k) => k <= 1 ? 1 : k * Factorial(k - 1);
    }
}
