using DataAcces.Data;
using Microsoft.AspNetCore.Mvc;
using TelegramBotExperiments.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly ApplicationDbContext _application;

    private readonly IBotService _bot;
    public WeatherForecastController(
        ILogger<WeatherForecastController> logger, 
        ApplicationDbContext context,
        IBotService bot
        )
    {
        _logger = logger;
        _application = context;
        _bot = bot;
    }


    
    [HttpGet(Name = "SendAll")]
    public async Task<ActionResult> SendAll(string message)
    {
        await _bot.SendAll(string.Empty);
        return Ok();
    }
}