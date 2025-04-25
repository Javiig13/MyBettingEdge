namespace Models.PredictiveModels
{
    public class ModelOutput
    {
        // Resultado del partido
        public decimal HomeWinProbability { get; set; }
        public decimal DrawProbability { get; set; }
        public decimal AwayWinProbability { get; set; }

        // Mercado de goles
        public decimal Over2_5Probability { get; set; }
        public decimal Under2_5Probability { get; set; }

        // Ambos marcan
        public decimal BTTSProbability { get; set; }
        public decimal BTTS_NoProbability { get; set; }

        // Estadísticas esperadas
        public decimal ExpectedGoals { get; set; }
        public decimal ExpectedCorners { get; set; }
        public decimal ExpectedCards { get; set; }

        // (Opcional) Scoring general del modelo
        public decimal Score { get; set; }
    }
}
