using DataAcces.Models;
using Quartz;
using Serilog;
using TelegramBotExperiments.Interfaces;

namespace TelegramBotExperiments.Jobs;

public class SendMessageJob:IJob
{
    private readonly List<TelegramUser> _users;
    private readonly IBotService _bot;

    public SendMessageJob(
        List<TelegramUser> users,
        IBotService bot)
    {
        _users = users;
        _bot = bot;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        foreach (var user in _users)
        {
            var text = user.Name + " " + "is the best";
            await _bot.SendMessageAsync(user.Id, text);
        }
        // _bot.SendMessageAsync(_users.First().Id, "djsjdl");
        Log.Logger.Information(_users.First().Name);
    }
}