namespace Data.External.DTOs
{
    public record InjuryReport(
        string TeamId,
        string PlayerName,
        string Position,
        string InjuryType,
        DateTime InjuryDate,
        DateTime? ExpectedReturn,
        bool IsStartingXI
    );
}
