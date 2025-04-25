using Core.Domain;
using Core.Enums;
using Data.External.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Data.External
{
    public class BetfairApiClient : IBettingApiClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BetfairApiClient> _logger;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly string _apiKey;
        private string _sessionToken = string.Empty;
        private bool _disposed;

        public BetfairApiClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BetfairApiClient> logger)
        {
            _apiKey = configuration["Betfair:ApiKey"]
                ?? throw new ArgumentNullException("Betfair:ApiKey");

            _httpClient = httpClientFactory.CreateClient("Betfair");
            _httpClient.BaseAddress = new Uri("https://api.betfair.com/exchange/");

            _logger = logger;

            // Configuración de política de reintentos (3 intentos con backoff exponencial)
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r =>
                    r.StatusCode is HttpStatusCode.RequestTimeout
                    or HttpStatusCode.TooManyRequests
                    or HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (response, delay, retryCount, context) =>
                    {
                        _logger.LogWarning($"Retry {retryCount} for {context.PolicyKey}." +
                            $"Delay: {delay.TotalSeconds}s. Status: {response.Result?.StatusCode}");
                    });
        }

        public async Task InitializeAsync(string username, string password)
        {
            var authPolicy = Policy
                .Handle<BetfairAuthException>()
                .WaitAndRetryAsync(2, retryAttempt =>
                    TimeSpan.FromSeconds(1));

            await authPolicy.ExecuteAsync(async () =>
            {
                _sessionToken = await AuthenticateAsync(username, password);
                _httpClient.DefaultRequestHeaders.Add("X-Authentication", _sessionToken);
                _httpClient.DefaultRequestHeaders.Add("X-Application", _apiKey);
            });
        }

        private async Task<string> AuthenticateAsync(string username, string password)
        {
            try
            {
                var authContent = new FormUrlEncodedContent(
                [
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                ]);

                var response = await _httpClient.PostAsync(
                    "https://identitysso.betfair.com/api/login",
                    authContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Authentication failed. Status: {StatusCode}",
                        response.StatusCode);
                    throw new BetfairAuthException("Invalid credentials");
                }

                return response.Headers.GetValues("X-Authentication").First();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical authentication failure");
                throw new BetfairAuthException("Authentication service unavailable", ex);
            }
        }

        public async Task<MatchOdds> GetLatestOddsAsync(string matchId)
        {
            ValidateSession();

            return await ExecuteApiCall<MatchOdds>(
                $"/odds/{WebUtility.UrlEncode(matchId)}",
                "GetLatestOdds");
        }

        public async Task<IEnumerable<LiveMatch>> GetLiveMatchesAsync(League league)
        {
            ValidateSession();

            return await ExecuteApiCall<IEnumerable<LiveMatch>>(
                $"/live-matches?league={league}",
                "GetLiveMatches");
        }

        public async Task<TeamStats> GetTeamStatsAsync(string teamId)
        {
            ValidateSession();

            return await ExecuteApiCall<TeamStats>(
                $"/teams/{WebUtility.UrlEncode(teamId)}/stats",
                "GetTeamStats");
        }

        public async Task<bool> PlaceBetAsync(BetRequest request)
        {
            ValidateSession();

            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                return await _httpClient.PostAsync("/place-bet", content);
            });

            return response.IsSuccessStatusCode;
        }

        private async Task<T> ExecuteApiCall<T>(string endpoint, string operationName)
        {
            try
            {
                var response = await _retryPolicy.ExecuteAsync(async () =>
                    await _httpClient.GetAsync(endpoint));

                response.EnsureSuccessStatusCode();

                return await DeserializeResponse<T>(response, operationName);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "{Operation} failed. Endpoint: {Endpoint}",
                    operationName, endpoint);
                throw new BetfairApiException($"API Error in {operationName}", ex);
            }
        }

        private async Task<T> DeserializeResponse<T>(
            HttpResponseMessage response,
            string operationName)
        {
            try
            {
                await using var stream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<T>(stream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new BetTypeConverter() }
                    }) ?? throw new InvalidDataException("Null API response");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Deserialization failed for {Operation}", operationName);
                throw new BetfairApiException("Invalid API response format", ex);
            }
        }

        private void ValidateSession()
        {
            if (string.IsNullOrEmpty(_sessionToken))
            {
                _logger.LogError("Attempted API call without valid session");
                throw new BetfairAuthException("Session not initialized");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }
    }

    // Excepciones personalizadas
    public class BetfairAuthException(string message, Exception inner = null) : Exception(message, inner)
    {
    }

    public class BetfairApiException(string message, Exception inner = null) : Exception(message, inner)
    {
    }

    // Conversor personalizado para BetType
    public class BetTypeConverter : JsonConverter<BetType>
    {
        public override BetType Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return reader.GetString() switch
            {
                "HOME_WIN" => BetType.HomeWin,
                "DRAW" => BetType.Draw,
                "AWAY_WIN" => BetType.AwayWin,
                "OVER_2_5" => BetType.Over2_5,
                "UNDER_2_5" => BetType.Under2_5,
                _ => throw new JsonException()
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            BetType value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToUpper());
        }
    }
}
