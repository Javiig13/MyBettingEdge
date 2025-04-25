using Core.Domain;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Core.Services
{
    public class NotificationService(string telegramBotToken, long chatId) : INotificationService
    {
        private readonly ITelegramBotClient _telegramBot = new TelegramBotClient(telegramBotToken);
        private const int MaxRetries = 3;

        public async Task SendBetRecommendationAsync(BetRecommendation recommendation)
        {
            var message = BuildRecommendationMessage(recommendation);
            await SendWithRetry(message);
        }

        public async Task SendAlertAsync(string message)
        {
            var formattedMessage = "$🚨 **ALERTA DEL SISTEMA** 🚨\n{ message}";
            await SendWithRetry(formattedMessage);
        }

        private static string BuildRecommendationMessage(BetRecommendation rec)
        {
            return $"""
            🎯 **RECOMENDACIÓN #{rec.RecommendationId}**
            ⚽ **Partido**: {rec.MatchId}
            📊 **Probabilidad Calculada**: {rec.CalculatedProbability:P1}
            🏷️ **Cuota**: {rec.Odds:N2}
            🔥 **Edge**: {rec.EdgePercentage:N2}% (Confianza: {rec.Confidence:N1}/5)
            💡 **Modelos**: {string.Join(", ", rec.SupportingModels)}
            🕒 **Hora Generación**: {rec.GeneratedAt:HH:mm UTC}
            """;
        }

        private async Task SendWithRetry(string message, int retryCount = 0)
        {
            try
            {
                await _telegramBot.SendMessage(
                    chatId: chatId,
                    text: message,
                    parseMode: ParseMode.MarkdownV2);
            }
            catch (Exception) when (retryCount < MaxRetries)
            {
                await Task.Delay(1000 * (retryCount + 1));
                await SendWithRetry(message, retryCount + 1);
            }
        }
    }
}
