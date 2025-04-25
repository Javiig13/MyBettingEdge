using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Data.Cache
{
    public static class RedisConnectionFactory
    {
        public static ConnectionMultiplexer Create(IConfiguration config)
            => ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { config["ConnectionStrings:Redis"] },
                AbortOnConnectFail = false,
                ConnectTimeout = 5000,
                SyncTimeout = 5000
            });
    }
}