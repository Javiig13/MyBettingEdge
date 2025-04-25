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
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", config["APIs:Football:Key"]);
            _logger = logger;
        }

        public async Task<MatchStatistics?> GetMatchStatsAsync(string matchId)
        {
            var response = await _httpClient.GetAsync($"/matches/{matchId}/stats");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Error obteniendo stats para {MatchId}. Código: {StatusCode}",
                    matchId, response.StatusCode);
                return null;
            }

            var stats = await response.Content.ReadFromJsonAsync<MatchStatistics>();

            if (stats is null)
            {
                _logger.LogError("No se pudo deserializar la respuesta de stats para {MatchId}", matchId);
                return null;
            }

            if (!stats.IsValid)
            {
                _logger.LogError("Datos inválidos para el partido {MatchId}", matchId);
                return null;
            }

            return stats;
        }
    }
}