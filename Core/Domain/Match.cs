using Core.Enums;

namespace Core.Domain
{
    public class Match
    {
        public string MatchId { get; init; } // Ej: "PL-1234-2023"
        public string HomeTeam { get; init; }
        public string AwayTeam { get; init; }
        public TeamStats? HomeTeamStats { get; private set; }
        public TeamStats? AwayTeamStats { get; private set; }
        public DateTime StartTime { get; init; }
        public League League { get; init; }
        public MatchStatus Status { get; private set; }

        // Estadísticas clave
        public double HomeExpectedGoals { get; private set; }
        public double AwayExpectedGoals { get; private set; }
        public int HomeRedCards { get; private set; }
        public int AwayRedCards { get; private set; }

        // Cuotas (Dictionary para acceso rápido)
        public Dictionary<BetType, decimal> Odds { get; private set; } = new();

        // Métodos
        public void UpdateLiveData(
            double homeXg,
            double awayXg,
            int homeRedCards,
            int awayRedCards,
            Dictionary<BetType, decimal> newOdds)
        {
            if (homeXg < 0 || awayXg < 0)
                throw new ArgumentException("xG no puede ser negativo");

            if (newOdds.Values.Any(o => o < 1.0m))
                throw new ArgumentException("Cuotas inválidas");

            HomeExpectedGoals = homeXg;
            AwayExpectedGoals = awayXg;
            HomeRedCards = homeRedCards;
            AwayRedCards = awayRedCards;
            Odds = newOdds;
            Status = MatchStatus.InPlay;
        }

        public void AssignTeamStats(TeamStats homeStats, TeamStats awayStats)
        {
            HomeTeamStats = homeStats;
            AwayTeamStats = awayStats;
        }

        public void FinishMatch()
        {
            Status = MatchStatus.Finished;
        }

        // Factory Method para creación segura
        public static Match Create(
            string matchId,
            string homeTeam,
            string awayTeam,
            DateTime startTime,
            League league)
        {
            if (string.IsNullOrWhiteSpace(matchId))
                throw new ArgumentException("MatchId no puede estar vacío");

            return new Match
            {
                MatchId = matchId,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                StartTime = startTime,
                League = league,
                Status = MatchStatus.PreMatch
            };
        }
    }

    public enum MatchStatus
    {
        PreMatch,
        InPlay,
        Finished,
        Postponed
    }
}
