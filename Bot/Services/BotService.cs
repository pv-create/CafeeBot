using DataAcces.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotExperiments.Interfaces;

namespace TelegramBotExperiments.Services;

public class BotService:IBotService
{
    private readonly List<TelegramUser> _users;
    private readonly ILogger<BotService> _logger;
    private readonly IConfiguration _configuration;
    
     static ITelegramBotClient bot = new TelegramBotClient("5847424294:AAFjHdNfKTuvYsW71museg7LrzcbUEyXOAk");

    public BotService(
        List<TelegramUser> users,
        ILogger<BotService> logger,
        IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
        _users = users;
    }
    
    
    
    private async Task HandleUpdateAsync(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                if (callbackQuery.Data == "callback_data_1")
                {
                    await bot.SendTextMessageAsync(
                        chatId: callbackQuery.Message.Chat.Id,
                        text: "Задачи на уравнения [URL](https://disk.yandex.ru/i/PDFvczv2r-LNvA)");
                }
                
                if (callbackQuery.Data == "callback_data_2")
                {
                    await bot.SendTextMessageAsync(
                        chatId: callbackQuery.Message.Chat.Id,
                        text: "Задачи на неравенства [URL](https://disk.yandex.ru/i/BVmloTJV2E2gWg)");
                }
            }
            
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var msg = update.Message;
                if (msg.Text.ToLower() == "/start")
                {
                    if (_users.All(x => x.Id != msg.Chat.Id))
                    {
                        var user = new TelegramUser()
                        {
                            Id = msg.Chat.Id,
                            Name = msg.Chat.Username
                        };
                        _users.Add(user);
                        _logger.LogInformation("добавлен пользователь {user} {Id}", user.Name, user.Id);
                    }
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Уравнения", "callback_data_1"),
                            InlineKeyboardButton.WithCallbackData("Неравенства", "callback_data_2")
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Геометрия", "callback_data_3")
                        }
                    });


                    var test = msg.Text;
                    await botClient.SendTextMessageAsync(
                        chatId: msg.Chat.Id,
                        text: "Choose one:",
                        replyMarkup: keyboard);
                }
            }
        }


    public async Task SendMessageAsync(long userId, string text)
    {
        await bot.SendTextMessageAsync(
            chatId: userId,
            text: text);
    }
    
    
    private  async Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }

    public void StartBot()
    {
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        
        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }
}