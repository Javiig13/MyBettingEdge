using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Domain;

namespace Data.Database.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(m => m.MatchId);

            builder.Property(m => m.StartTime)
                .HasColumnType("timestamp with time zone");

            builder.HasIndex(m => new { m.HomeTeam, m.AwayTeam, m.StartTime });
        }
    }
}