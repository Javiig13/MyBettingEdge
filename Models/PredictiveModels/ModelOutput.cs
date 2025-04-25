using Microsoft.ML.Data;

namespace Models.PredictiveModels
{
    public record ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool IsValueBet { get; set; }

        [ColumnName("Probability")]
        public float Confidence { get; set; }
    }
}
