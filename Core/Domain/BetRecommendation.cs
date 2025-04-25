using Core.Enums;

namespace Core.Domain
{
    /// <summary>
    /// Recomendación generada por el sistema para apostar
    /// </summary>
    public class BetRecommendation
    {
        public string RecommendationId { get; } = Guid.NewGuid().ToString();
        public required string MatchId { get; init; }
        public required BetType BetType { get; init; }
        public decimal Odds { get; init; }
        public decimal CalculatedProbability { get; init; }
        public decimal EdgePercentage { get; set; } // Ventaja sobre la casa
        public decimal Confidence { get; init; } // Nueva propiedad

        // Detalles de modelos
        public List<string> SupportingModels { get; } = []; // Ej: ["XGBoost", "Poisson"]
        public DateTime GeneratedAt { get; } = DateTime.UtcNow;

        // Métodos
        public void CalculateEdge(decimal bookmakerImplicitProbability)
        {
            if (bookmakerImplicitProbability <= 0 || bookmakerImplicitProbability >= 1)
                throw new ArgumentOutOfRangeException(nameof(bookmakerImplicitProbability));

            EdgePercentage = (CalculatedProbability - bookmakerImplicitProbability) * 100;
        }

        public bool IsHighConfidence() =>
            EdgePercentage >= 7.0m &&
            SupportingModels.Count >= 2;

        // Factory Method para apuestas simples
        public static BetRecommendation CreateSimple(
            string matchId,
            BetType betType,
            decimal odds,
            decimal probability,
            IEnumerable<string> models,
            decimal confidence)
        {
            return new BetRecommendation
            {
                MatchId = matchId,
                BetType = betType,
                Odds = odds,
                CalculatedProbability = probability,
                Confidence = confidence
            }.WithModels(models);
        }

        private BetRecommendation WithModels(IEnumerable<string> models)
        {
            SupportingModels.AddRange(models);
            return this;
        }
    }
}
