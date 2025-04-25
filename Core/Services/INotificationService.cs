using Core.Domain;

namespace Core.Services
{
    public interface INotificationService
    {
        Task SendBetRecommendationAsync(BetRecommendation recommendation);
        Task SendAlertAsync(string message);
    }
}
