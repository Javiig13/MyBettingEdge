using Data.Cache.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Data.Cache.Services
{
    public class RedisCacheService(IConnectionMultiplexer connection) : ICacheService
    {
        private readonly IDatabase _redis = connection.GetDatabase();

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _redis.StringGetAsync(key);
            return value.HasValue
                ? JsonSerializer.Deserialize<T>(value!)
                : default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serialized = JsonSerializer.Serialize(value);
            await _redis.StringSetAsync(key, serialized, expiry);
        }

        public async Task RemoveAsync(string key)
            => await _redis.KeyDeleteAsync(key);

        public async Task ClearAllAsync()
        {
            var endpoints = connection.GetEndPoints();
            var server = connection.GetServer(endpoints.First());
            await server.FlushAllDatabasesAsync();
        }
    }
}