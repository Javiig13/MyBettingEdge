using Core.Domain;
using Core.Enums;
using Strategies.Core;
using Strategies.ValueBet;

namespace Strategies.TournamentSpecific
{
    public class DerbyStrategy : IBettingStrategy
    {
        private readonly IEdgeCalculator _edgeCalculator;
        private TournamentConfiguration _config;

        public string StrategyName => "Derby Special";

        public DerbyStrategy(IEdgeCalculator edgeCalculator)
        {
            _edgeCalculator = edgeCalculator;
            _config = new TournamentConfiguration
            {
                MinEdge = 0.03m,
                MaxOdds = 4.0m,
                AllowedBetTypes =
                [
                    BetType.HomeWin,
                    BetType.AwayWin,
                    BetType.Draw,
                    BetType.Over2_5,
                    BetType.BTTS
                ]
            };
        }

        public void UpdateConfiguration(StrategyConfiguration config)
        {
            if (config is TournamentConfiguration tournamentConfig)
            {
                _config = tournamentConfig;
            }
            else
            {
                _config = new TournamentConfiguration
                {
                    MinEdge = config.MinEdge,
                    MaxOdds = config.MaxOdds,
                    AllowedLeagues = config.AllowedLeagues,
                    AllowedBetTypes = config.AllowedBetTypes
                };
            }
        }

        public async Task<StrategyResult> ExecuteAsync(IEnumerable<MatchAnalysis> matches)
        {
            var derbyMatches = matches.Where(IsDerbyMatch);
            var recommendations = new List<BetRecommendation>();

            foreach (var match in derbyMatches)
            {
                foreach (var betType in GetDerbyBetTypes())
                {
                    if (_edgeCalculator.HasValue(match, betType, _config.MinEdge))
                    {
                        recommendations.Add(CreateRecommendation(match, betType));
                    }
                }
            }

            return new StrategyResult
            {
                StrategyName = StrategyName,
                Recommendations = recommendations
            };
        }

        private bool IsDerbyMatch(MatchAnalysis match)
        {
            // Implementación de lógica para detectar derbis
            var derbyPairs = new List<(string, string)>
            {
                ("Real Madrid", "Barcelona"),
                ("Manchester United", "Liverpool"),
                ("AC Milan", "Inter Milan")
            };

            return derbyPairs.Any(pair =>
                (match.HomeTeam == pair.Item1 && match.AwayTeam == pair.Item2) ||
                (match.HomeTeam == pair.Item2 && match.AwayTeam == pair.Item1));
        }

        private IEnumerable<BetType> GetDerbyBetTypes()
        {
            yield return BetType.HomeWin;
            yield return BetType.AwayWin;
            yield return BetType.Draw;
            yield return BetType.Over2_5;
            yield return BetType.BTTS;
        }

        private BetRecommendation CreateRecommendation(MatchAnalysis match, BetType betType)
        {
            var edge = _edgeCalculator.CalculateEdge(match, betType);
            return new BetRecommendation
            {
                MatchId = match.MatchId,
                BetType = betType,
                Odds = match.Odds[betType],
                CalculatedProbability = edge.Probability,
                EdgePercentage = edge.Percentage,
                Confidence = edge.Probability * 100m
            };
        }
    }
}
