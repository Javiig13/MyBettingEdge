using Core.Enums;

namespace Data.External.DTOs
{
    public record BookmakerOdds(
        string BookmakerName,  // Cambiado de Bookmaker a BookmakerName
        BetType BetType,
        decimal Odds,
        DateTime UpdatedAt
    )
    {
        public bool IsOutdated => (DateTime.UtcNow - UpdatedAt) > TimeSpan.FromMinutes(5);

        // Método para obtener odds por tipo
        public decimal GetOddsForBetType(BetType betType)
        {
            return BetType == betType ? Odds : 0m;
        }
    }
}
