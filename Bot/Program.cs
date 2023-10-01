using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotExperiments.Interfaces;
using TelegramBotExperiments.Services;

namespace TelegramBotExperiments
{

    class Program
    {
        // static ITelegramBotClient bot = new TelegramBotClient("5847424294:AAFjHdNfKTuvYsW71museg7LrzcbUEyXOAk");
        
        
        // public static async Task HandleUpdateAsync(
        //     ITelegramBotClient botClient,
        //     Update update,
        //     CancellationToken cancellationToken)
        // {
        //     if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        //     {
        //         var callbackQuery = update.CallbackQuery;
        //         if (callbackQuery.Data == "callback_data_1")
        //         {
        //             await bot.SendTextMessageAsync(
        //                 chatId: callbackQuery.Message.Chat.Id,
        //                 text: "Задачи на уравнения [URL](https://docs.yandex.ru/docs/view?url=ya-disk%3A%2F%2F%2Fdisk%2FЗадачи%2FEquations.pdf&name=Equations.pdf&uid=724085872)");
        //         }
        //         
        //         if (callbackQuery.Data == "callback_data_2")
        //         {
        //             await bot.SendTextMessageAsync(
        //                 chatId: callbackQuery.Message.Chat.Id,
        //                 text: "Задачи на неравенства [URL](https://docs.yandex.ru/docs/view?url=ya-disk%3A%2F%2F%2Fdisk%2FЗадачи%2FNeraw.pdf&name=Neraw.pdf&uid=7240858722)");
        //         }
        //     }
        //     
        //     if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        //     {
        //         var msg = update.Message;
        //         if (msg.Text.ToLower() == "/start")
        //         {
        //             var keyboard = new InlineKeyboardMarkup(new[]
        //             {
        //                 new []
        //                 {
        //                     InlineKeyboardButton.WithCallbackData("Уравнения", "callback_data_1"),
        //                     InlineKeyboardButton.WithCallbackData("Неравенства", "callback_data_2")
        //                 },
        //                 new []
        //                 {
        //                     InlineKeyboardButton.WithCallbackData("Геометрия", "callback_data_3")
        //                 }
        //             });
        //
        //
        //             var test = msg.Text;
        //             await botClient.SendTextMessageAsync(
        //                 chatId: msg.Chat.Id,
        //                 text: "Choose one:",
        //                 replyMarkup: keyboard);
        //         }
        //     }
        // }

        // public static async Task HandleErrorAsync(
        //     ITelegramBotClient botClient,
        //     Exception exception,
        //     CancellationToken cancellationToken)
        // {
        //     // Некоторые действия
        //     Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        // }


        static void Main(string[] args)
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
                    services.AddTransient<IBotService, BotService>();
                })
                .UseSerilog()
                .Build();

            var bot = ActivatorUtilities.CreateInstance<BotService>(host.Services);

            bot.StartBot();

            // var cts = new CancellationTokenSource();
            // var cancellationToken = cts.Token;
            // var receiverOptions = new ReceiverOptions
            // {
            //     AllowedUpdates = { }, // receive all update types
            // };
            // bot.StartReceiving(
            //     HandleUpdateAsync,
            //     HandleErrorAsync,
            //     receiverOptions,
            //     cancellationToken
            // );
            // Console.ReadLine();
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