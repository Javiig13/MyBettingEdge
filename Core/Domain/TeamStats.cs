using Core.Enums;

namespace Core.Domain
{
    /// <summary>
    /// Estadísticas completas de un equipo para múltiples contextos (Domain + ML)
    /// </summary>
    public class TeamStats
    {
        // Identificación básica
        public required string TeamId { get; init; }
        public required string TeamName { get; init; }
        public required League MainLeague { get; init; }

        // Rendimiento reciente (máximo 10 partidos)
        private readonly Queue<MatchPerformance> _lastPerformances = new(10);
        public IReadOnlyCollection<MatchPerformance> LastPerformances => _lastPerformances;

        // Métricas principales (promedios móviles)
        public double MovingAvg_GoalsScored { get; private set; }
        public double MovingAvg_GoalsConceded { get; private set; }
        public double MovingAvg_xG { get; private set; }
        public double MovingAvg_xGA { get; private set; }

        // Métricas adicionales para modelos ML
        public double CurrentEloRating { get; set; }
        public double AvgCornersFor { get; private set; }
        public double AvgCornersAgainst { get; private set; }
        public double AvgCardsFor { get; private set; }
        public double AvgCardsAgainst { get; private set; }

        // Método único de actualización
        public void UpdateStats(MatchPerformance performance)
        {
            // Mantener máximo 10 partidos
            if (_lastPerformances.Count == 10)
                _lastPerformances.Dequeue();

            _lastPerformances.Enqueue(performance);

            // Actualizar todos los promedios
            UpdateMovingAverages();
            UpdateAdditionalMetrics();
        }

        private void UpdateMovingAverages()
        {
            MovingAvg_GoalsScored = _lastPerformances.Average(p => p.GoalsScored);
            MovingAvg_GoalsConceded = _lastPerformances.Average(p => p.GoalsConceded);
            MovingAvg_xG = _lastPerformances.Average(p => p.ExpectedGoals);
            MovingAvg_xGA = _lastPerformances.Average(p => p.ExpectedGoalsAgainst);
        }

        private void UpdateAdditionalMetrics()
        {
            AvgCornersFor = _lastPerformances.Average(p => p.CornersFor);
            AvgCornersAgainst = _lastPerformances.Average(p => p.CornersAgainst);
            AvgCardsFor = _lastPerformances.Average(p => p.YellowCards + p.RedCards);
            AvgCardsAgainst = _lastPerformances.Average(p => p.OpponentCards);
        }

        // Factory Method mejorado
        public static TeamStats Create(string teamId, string teamName, League league, double initialElo = 1500)
        {
            return new TeamStats
            {
                TeamId = teamId,
                TeamName = teamName,
                MainLeague = league,
                CurrentEloRating = initialElo
            };
        }
    }

    /// <summary>
    /// Rendimiento detallado en un partido (ampliado para ML)
    /// </summary>
    public record MatchPerformance(
        string MatchId,
        int GoalsScored,
        int GoalsConceded,
        double ExpectedGoals,
        double ExpectedGoalsAgainst,
        int CornersFor,
        int CornersAgainst,
        int YellowCards,
        int RedCards,
        int OpponentCards,
        bool IsHomeGame
    );
}