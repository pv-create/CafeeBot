using Quartz;
using TelegramBotExperiments.Interfaces;

namespace WebApi.Jobs;

public class StartBotJob:IJob
{
    private readonly ILogger<StartBotJob> _logger;
    private readonly IBotService _botService;
    public StartBotJob(
        ILogger<StartBotJob> logger,
        IBotService botService)
    {
        _logger = logger;
        _botService = botService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("StartBotJob");
        
        Thread myThread = new Thread(() =>
        {
            _botService.StartBot();
        });
        myThread.Start();

        await Task.CompletedTask;
    }
}