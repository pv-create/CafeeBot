using DataAcces.Models;
using Quartz;
using TelegramBotExperiments.Interfaces;

namespace WebApi.Jobs;

public class SendMessageJob:IJob
{
    private readonly List<TelegramUser> _users;
    private readonly IBotService _bot;
    private readonly ILogger<SendMessageJob> _logger;

    public SendMessageJob(
        List<TelegramUser> users,
        IBotService bot,
        ILogger<SendMessageJob> logger)
    {
        _users = users;
        _bot = bot;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        foreach (var user in _users)
        {
            var text = "[URL](https://disk.yandex.ru/i/PDFvczv2r-LNvA)";
            await _bot.SendMessageAsync(user.Id, text);
            _logger.LogInformation("отправлено сообщение пользователю {user}", user.Name);
        }
    }
}