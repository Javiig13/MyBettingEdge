using Core.Domain;
using Data.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Data.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<TeamStats> TeamStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MatchConfiguration());
            modelBuilder.ApplyConfiguration(new BetConfiguration());
        }
    }
}