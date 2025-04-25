using Core.Services;
using Data.Cache;
using Data.Cache.Interfaces;
using Data.Cache.Services;
using Data.Database;
using Data.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyBettingEdge.Data.Database.Repositories;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.Local.json", optional: false, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        IConfiguration configuration = context.Configuration;

        // ⛓ Base de datos PostgreSQL
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

        // 🔁 Repositorios
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<ITeamStatsRepository, TeamStatsRepository>();
        services.AddScoped<IRefereeStatsRepository, RefereeStatsRepository>();
        services.AddScoped<IBetRepository, BetRepository>();

        // 🧠 Servicios de caché Redis
        services.AddSingleton<RedisConnectionFactory>();
        services.AddSingleton<ICacheService, RedisCacheService>();

        // 📢 Notificaciones (Telegram u otras)
        services.AddSingleton<INotificationService, NotificationService>();

        // 💡 Logging
        services.AddLogging(config =>
        {
            config.AddConsole();
            config.SetMinimumLevel(LogLevel.Information);
        });

        // 🏃 Tareas / servicios personalizados
        // services.AddHostedService<PredictionRunner>(); // si lo activas luego
    });

var host = builder.Build();

// ⚠️ Aplica migraciones automáticamente (opcional)
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Solo si usas migraciones de EF Core
}

await host.RunAsync();
