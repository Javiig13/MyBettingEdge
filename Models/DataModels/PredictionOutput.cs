namespace Models.DataModels
{
    public record PredictionOutput
    {
        public decimal HomeWinProbability { get; init; }
        public decimal DrawProbability { get; init; }
        public decimal AwayWinProbability { get; init; }
        public decimal Over2_5Probability { get; init; }
        public decimal Under2_5Probability { get; init; }
        public decimal BTTSProbability { get; init; }
        public decimal BTTS_NoProbability { get; init; }
        public decimal ExpectedGoals { get; init; }
        public decimal ExpectedCorners { get; init; }
        public decimal ExpectedCards { get; init; }

        public static PredictionOutput Combine(
            PredictionOutput poisson,
            PredictionOutput elo,
            decimal poissonWeight = 0.4m,
            decimal eloWeight = 0.6m)
        {
            return new PredictionOutput
            {
                HomeWinProbability = (poisson.HomeWinProbability * poissonWeight) + (elo.HomeWinProbability * eloWeight),
                DrawProbability = (poisson.DrawProbability * poissonWeight) + (elo.DrawProbability * eloWeight),
                AwayWinProbability = (poisson.AwayWinProbability * poissonWeight) + (elo.AwayWinProbability * eloWeight),
                Over2_5Probability = (poisson.Over2_5Probability * poissonWeight) + (elo.Over2_5Probability * eloWeight),
                Under2_5Probability = (poisson.Under2_5Probability * poissonWeight) + (elo.Under2_5Probability * eloWeight),
                BTTSProbability = (poisson.BTTSProbability * poissonWeight) + (elo.BTTSProbability * eloWeight),
                BTTS_NoProbability = (poisson.BTTS_NoProbability * poissonWeight) + (elo.BTTS_NoProbability * eloWeight),
                ExpectedGoals = (poisson.ExpectedGoals * poissonWeight) + (elo.ExpectedGoals * eloWeight),
                ExpectedCorners = (poisson.ExpectedCorners * poissonWeight) + (elo.ExpectedCorners * eloWeight),
                ExpectedCards = (poisson.ExpectedCards * poissonWeight) + (elo.ExpectedCards * eloWeight)
            };
        }
    }
}
