using Data.External.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Data.External
{
    public class ApiFootballClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiFootballClient> _logger;

        public ApiFootballClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration config,
            ILogger<ApiFootballClient> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiFootball");
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key",
                config["APIs:Football:Key"]);
            _logger = logger;
        }

        public async Task<MatchStatistics> GetMatchStatsAsync(string matchId)
        {
            var response = await _httpClient.GetAsync($"/matches/{matchId}/stats");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Error obteniendo stats para {MatchId}. Código: {StatusCode}",
                                 matchId, response.StatusCode);
                return MatchStatistics.Empty;
            }

            var stats = await response.Content.ReadFromJsonAsync<MatchStatistics>();

            return stats?.IsValid() == true
                ? stats
                : throw new InvalidDataException("Datos inválidos de API-Football");
        }

        // Método auxiliar en MatchStatistics
        public bool IsValid() =>
            ExpectedGoalsHome >= 0 &&
            ExpectedGoalsAway >= 0 &&
            TotalShots > 0;
    }
}
