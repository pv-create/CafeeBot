using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using TelegramBotExperiments.Interfaces;
using TelegramBotExperiments.Jobs;

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
        // IJob myJob = new SendMessageJob();
        // JobDetailImpl jobDetail = new JobDetailImpl("Job1", "Group1", myJob.GetType());
        // CronTriggerImpl trigger = new CronTriggerImpl("Trigger1", "Group1", "0 * * ? * *"); //run every minute between the hours of 8am and 5pm
        // _scheduler.ScheduleJob(jobDetail, trigger);
        // DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
        // Console.WriteLine("Next Fire Time:" + nextFireTime.Value);
    }
}