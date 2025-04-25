using Data.Cache.Interfaces;
using Data.Cache.Services;
using Data.Cache;
using Data.Database.Repositories;
using Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Data
{
    public static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("PostgreSQL")));

            services.AddScoped<IMatchRepository, MatchRepository>();
            return services;
        }

        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration config)
        {
            var redis = RedisConnectionFactory.Create(config);
            services.AddSingleton(redis);
            services.AddSingleton<ICacheService, RedisCacheService>();
            return services;
        }
    }
}