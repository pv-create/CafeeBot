using DataAcces.Data;
using DataAcces.Models;
using Microsoft.AspNetCore.Mvc;
using TelegramBotExperiments.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly ApplicationDbContext _application;

    private readonly IBotService _bot;
    
    private readonly ApplicationDbContext _applicationContext;
    public WeatherForecastController(
        ILogger<WeatherForecastController> logger, 
        ApplicationDbContext context,
        IBotService bot,
        ApplicationDbContext applicationContext
        )
    {
        _logger = logger;
        _application = context;
        _bot = bot;
        _applicationContext = applicationContext;
    }


    
    [HttpGet(Name = "SendAll")]
    public async Task<ActionResult> SendAll(string message)
    {
        await _bot.SendAll(message);
        return Ok();
    }

    [HttpGet(Name = "GetUsers")]
    public ActionResult<List<TelegramUser>> GetUsers()
    {
        return Ok(_applicationContext.Users.ToList());
    }
}