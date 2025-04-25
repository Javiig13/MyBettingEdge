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
        public decimal Stake { get; init; }
        // Detalles de modelos
        public List<string> SupportingModels { get; private set; } = []; // Ej: ["XGBoost", "Poisson"]
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
            decimal confidence,
            decimal stake,
            decimal edge)
        {
            return new BetRecommendation
            {
                MatchId = matchId,
                BetType = betType,
                Odds = odds,
                CalculatedProbability = probability,
                Confidence = confidence,
                Stake = stake,
                EdgePercentage = edge,
                SupportingModels = models.ToList()
            };
        }

        public BetRecommendation WithModels(IEnumerable<string> models)
        {
            SupportingModels = models.ToList();
            return this;
        }
    }
}
