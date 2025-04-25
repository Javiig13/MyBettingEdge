using Core.Domain;
using Core.Enums;
using Strategies.Core;
using Strategies.ValueBet;

namespace Strategies.TournamentSpecific
{
    public class ChampionsLeagueStrategy : IBettingStrategy
    {
        private readonly IEdgeCalculator _edgeCalculator;
        private TournamentConfiguration _config;

        public string StrategyName => "Champions League Special";

        public ChampionsLeagueStrategy(IEdgeCalculator edgeCalculator)
        {
            _edgeCalculator = edgeCalculator;
            _config = new TournamentConfiguration
            {
                MinEdge = 0.05m,
                MaxOdds = 3.5m,
                KnockoutPhaseMultiplier = 1.2m,
                GroupPhaseMultiplier = 0.8m,
                FinalMultiplier = 1.5m,
                AllowedBetTypes =
                [
                    BetType.HomeWin,
                    BetType.AwayWin,
                    BetType.Over2_5,
                    BetType.BTTS
                ]
            };
        }

        public void UpdateConfiguration(StrategyConfiguration config)
        {
            if (config is TournamentConfiguration tournamentConfig)
            {
                _config = tournamentConfig with
                {
                    // Mantener los multiplicadores específicos si no se proporcionan
                    KnockoutPhaseMultiplier = tournamentConfig.KnockoutPhaseMultiplier != default
                        ? tournamentConfig.KnockoutPhaseMultiplier
                        : _config.KnockoutPhaseMultiplier,
                    GroupPhaseMultiplier = tournamentConfig.GroupPhaseMultiplier != default
                        ? tournamentConfig.GroupPhaseMultiplier
                        : _config.GroupPhaseMultiplier,
                    FinalMultiplier = tournamentConfig.FinalMultiplier != default
                        ? tournamentConfig.FinalMultiplier
                        : _config.FinalMultiplier
                };
            }
            else
            {
                _config = _config with
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
            var filteredMatches = matches.Where(m => m.League == League.ChampionsLeague);
            var recommendations = new List<BetRecommendation>();

            foreach (var match in filteredMatches)
            {
                decimal phaseMultiplier = GetPhaseMultiplier((ChampionsLeagueStage)match.Stage);

                foreach (var betType in GetRecommendedBetTypes((ChampionsLeagueStage)match.Stage))
                {
                    if (_edgeCalculator.HasValue(match, betType, _config.MinEdge * phaseMultiplier))
                    {
                        var recommendation = CreateRecommendation(match, betType, phaseMultiplier);
                        recommendations.Add(recommendation);
                    }
                }
            }

            return new StrategyResult
            {
                StrategyName = StrategyName,
                Recommendations = recommendations
            };
        }

        private decimal GetPhaseMultiplier(ChampionsLeagueStage stage)
        {
            return stage switch
            {
                ChampionsLeagueStage.Group => _config.GroupPhaseMultiplier,
                ChampionsLeagueStage.RoundOf16 => _config.KnockoutPhaseMultiplier,
                ChampionsLeagueStage.QuarterFinal => _config.KnockoutPhaseMultiplier,
                ChampionsLeagueStage.SemiFinal => _config.KnockoutPhaseMultiplier,
                ChampionsLeagueStage.Final => _config.FinalMultiplier,
                _ => 1.0m
            };
        }

        private IEnumerable<BetType> GetRecommendedBetTypes(ChampionsLeagueStage stage)
        {
            yield return BetType.HomeWin;
            yield return BetType.AwayWin;

            if (stage != ChampionsLeagueStage.Final)
            {
                yield return BetType.Over2_5;
                yield return BetType.BTTS;
            }
        }

        private BetRecommendation CreateRecommendation(MatchAnalysis match, BetType betType, decimal multiplier)
        {
            var edge = _edgeCalculator.CalculateEdge(match, betType);
            return new BetRecommendation
            {
                MatchId = match.MatchId,
                BetType = betType,
                Odds = match.Odds[betType],
                CalculatedProbability = edge.Probability * multiplier,
                EdgePercentage = edge.Percentage * multiplier,
                Confidence = edge.Probability * 100m * multiplier
            };
        }
    }
}
