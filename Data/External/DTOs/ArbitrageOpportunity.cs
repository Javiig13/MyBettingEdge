namespace Data.External.DTOs
{
    public record ArbitrageOpportunity(
        string BackBookmaker,
        decimal BackOdds,
        string LayBookmaker,
        decimal LayOdds);
}
