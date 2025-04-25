using Core.Enums;

namespace Data.External.DTOs
{
    public record MatchOdds(
    string MatchId,
    decimal HomeWin,
    decimal Draw,
    decimal AwayWin,
    decimal Over2_5,
    decimal Under2_5,
    DateTime LastUpdated)
    {
        // Método para obtener odds por tipo de apuesta
        public decimal ForBetType(BetType betType)
        {
            return betType switch
            {
                BetType.HomeWin => HomeWin,
                BetType.Draw => Draw,
                BetType.AwayWin => AwayWin,
                BetType.Over2_5 => Over2_5,
                BetType.Under2_5 => Under2_5,
                _ => throw new ArgumentException("Tipo de apuesta no soportado")
            };
        }
    }
}
