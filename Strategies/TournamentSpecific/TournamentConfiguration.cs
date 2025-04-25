using Core.Enums;
using Strategies.Core;

namespace Strategies.TournamentSpecific
{
    public record TournamentConfiguration : StrategyConfiguration
    {
        public decimal KnockoutPhaseMultiplier { get; init; } = 1.0m;
        public decimal GroupPhaseMultiplier { get; init; } = 1.0m;
        public decimal FinalMultiplier { get; init; } = 1.0m;
        public HashSet<ChampionsLeagueStage> ApplicableStages { get; init; } =
        [
            ChampionsLeagueStage.Group,
            ChampionsLeagueStage.RoundOf16,
            ChampionsLeagueStage.QuarterFinal,
            ChampionsLeagueStage.SemiFinal,
            ChampionsLeagueStage.Final
        ];
    }
}
