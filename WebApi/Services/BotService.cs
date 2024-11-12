using DataAcces.Data;
using DataAcces.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotExperiments.Interfaces;

namespace WebApi.Services;

public class BotService:IBotService
{
    private readonly List<TelegramUser> _users;
    private readonly ILogger<BotService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    
     static ITelegramBotClient bot = new TelegramBotClient("5847424294:AAFjHdNfKTuvYsW71museg7LrzcbUEyXOAk");

    public BotService(
        List<TelegramUser> users,
        ILogger<BotService> logger,
        IConfiguration configuration,
        IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _users = users;
        _contextFactory = contextFactory;
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
                    using (var context = await _contextFactory.CreateDbContextAsync())
                    {
                        if (context.Users.All(x => x.Id != msg.Chat.Id))
                        {
                            var user = new TelegramUser()
                            {
                                Id = msg.Chat.Id,
                                Name = msg.Chat.Username
                            };
                            context.Users.Add(user);
                            await context.SaveChangesAsync();
                            _logger.LogInformation("добавлен пользователь {user} {Id}", user.Name, user.Id);
                        }
                    }
                    
                    
                    
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithWebApp(
                                text: "Открыть веб-приложение",
                                webApp: new WebAppInfo { Url = "https://catcoffe.ru" }
                            )
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
            AllowedUpdates = { },
        };
        
        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }

    public async Task SendAll(string message)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            foreach (var user in context.Users.ToList())
            {
                var text = message;
                await SendMessageAsync(user.Id, text);
                _logger.LogInformation("отправлено сообщение пользователю {user}", user.Name);
            }

        }
    }
}