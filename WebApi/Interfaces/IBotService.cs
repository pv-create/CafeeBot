namespace TelegramBotExperiments.Interfaces;

public interface IBotService
{
    void StartBot();
    Task SendMessageAsync(long userId, string text);
    Task SendAll(string message);
}