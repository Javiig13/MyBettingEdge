using Core.Enums;

namespace Models.Ensemble
{
    public record ModelPrediction
    {
        public required string ModelName { get; init; }
        public required PredictionProbabilities Probabilities { get; init; }
        public DateTime PredictionTime { get; init; } = DateTime.UtcNow;
    }

    public record PredictionProbabilities
    {
        // Probabilidades básicas de resultado
        public decimal HomeWin { get; init; }
        public decimal Draw { get; init; }
        public decimal AwayWin { get; init; }

        // Mercados de goles
        public decimal Over2_5 { get; init; }
        public decimal Under2_5 { get; init; }

        // Both Teams To Score
        public decimal BTTS { get; init; }
        public decimal BTTS_No { get; init; }

        // Estadísticas esperadas
        public decimal ExpectedGoals { get; init; }        // xG total esperado en el partido
        public decimal ExpectedCorners { get; init; }     // Número esperado de corners
        public decimal ExpectedCards { get; init; }       // Número esperado de tarjetas

        // Métodos útiles
        public decimal GetProbabilityForBetType(BetType betType)
        {
            return betType switch
            {
                BetType.HomeWin => HomeWin,
                BetType.Draw => Draw,
                BetType.AwayWin => AwayWin,
                BetType.Over2_5 => Over2_5,
                BetType.Under2_5 => Under2_5,
                BetType.BTTS => BTTS,
                BetType.BTTS_No => BTTS_No,
                _ => throw new ArgumentException("Tipo de apuesta no soportado")
            };
        }

        public bool HasValue()
        {
            return HomeWin + Draw + AwayWin > 0.9m; // Validación básica
        }
    }
}
