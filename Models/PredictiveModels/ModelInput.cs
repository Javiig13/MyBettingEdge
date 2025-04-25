using Microsoft.ML.Data;

namespace Models.PredictiveModels
{
    public class ModelInput
    {
        // Features básicas
        [LoadColumn(0), ColumnName("home_elo")]
        public float HomeElo { get; set; }

        [LoadColumn(1), ColumnName("away_elo")]
        public float AwayElo { get; set; }

        // Forma reciente
        [LoadColumn(2), ColumnName("home_form_last5")]
        public float HomeFormLast5 { get; set; }

        [LoadColumn(3), ColumnName("away_form_last5")]
        public float AwayFormLast5 { get; set; }

        // Estadísticas ofensivas/defensivas
        [LoadColumn(4), ColumnName("home_avg_goals_scored")]
        public float HomeAvgGoalsScored { get; set; }

        [LoadColumn(5), ColumnName("away_avg_goals_scored")]
        public float AwayAvgGoalsScored { get; set; }

        [LoadColumn(6), ColumnName("home_avg_goals_conceded")]
        public float HomeAvgGoalsConceded { get; set; }

        [LoadColumn(7), ColumnName("away_avg_goals_conceded")]
        public float AwayAvgGoalsConceded { get; set; }

        // ... 40+ features adicionales (lesiones, clima, árbitro, etc.)

        [LoadColumn(49), ColumnName("ref_avg_red_cards")]
        public float RefAvgRedCards { get; set; }

        [LoadColumn(50), ColumnName("is_derby")]
        public bool IsDerby { get; set; }
    }
}
