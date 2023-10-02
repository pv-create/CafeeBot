using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DataAcces.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotExperiments.Interfaces;
using TelegramBotExperiments.Jobs;
using TelegramBotExperiments.Services;

namespace TelegramBotExperiments
{

    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            
            
            
            Log.Logger.Information("Bot Started");
            
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<List<TelegramUser>>();
                    services.AddTransient<IBotService, BotService>();
                    services.AddTransient<IShedulerService, ShedulerService>();

                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();

                        JobKey jobKey = new("Send notifications");
                        q.AddJob<SendMessageJob>(options =>
                            options.WithIdentity(jobKey));
                        q.AddTrigger(options => options
                            .ForJob(jobKey)
                            .WithIdentity(jobKey.Name + " trigger")
                            .WithCronSchedule("0 * * ? * *"));
                    });
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                })
                .UseSerilog()
                .Build();

            var bot = ActivatorUtilities.CreateInstance<BotService>(host.Services);
            
            
            Thread myThread = new Thread(() =>
            {
                bot.StartBot();
            });
            myThread.Start();
            
            host.Run();
            //var shd = ActivatorUtilities.CreateInstance<ShedulerService>(host.Services);

            //await shd.StartSheduler();
           // Console.ReadKey();
          
            
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.json.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Productiom"}.json",
                    optional: true)
                .AddEnvironmentVariables();
        }
    }
}