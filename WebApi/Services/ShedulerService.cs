using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using TelegramBotExperiments.Interfaces;
using WebApi.Jobs;

namespace TelegramBotExperiments.Services;

public class ShedulerService:IShedulerService
{
    private readonly ILogger<ShedulerService> _logger;
    private static IScheduler _scheduler;
    
    public ShedulerService(
        ILogger<ShedulerService> logger)
    {
        _logger = logger;
    }
    
    public async Task StartSheduler()
    {
        ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
        _scheduler = await  schedulerFactory.GetScheduler();
        _scheduler.Start();
        
        AddJob();
        _logger.LogInformation("sheduler start");
    }
    
    public static void AddJob()
    {
        
    }
}