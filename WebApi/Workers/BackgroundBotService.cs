using TelegramBotExperiments.Interfaces;

public class BotBackgroundService : BackgroundService
{
    private readonly ILogger<BotBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public BotBackgroundService(
        ILogger<BotBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            
            var botService = scope.ServiceProvider.GetRequiredService<IBotService>();
            Thread myThread = new Thread(() =>
            {
                botService.StartBot();
            });
            myThread.Start();
            
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in bot background service");
        }
    }
}