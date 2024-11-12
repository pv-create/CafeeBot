using DataAcces.Data;
using DataAcces.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;
using TelegramBotExperiments.Interfaces;
using TelegramBotExperiments.Services;
using WebApi.Jobs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<List<TelegramUser>>();
builder.Services.AddTransient<IBotService, BotService>();
builder.Services.AddTransient<IShedulerService, ShedulerService>();

builder.Services.AddHostedService<BotBackgroundService>();
builder.Services.AddQuartz(q =>
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
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString,  b => b.MigrationsAssembly("WebApi")));

builder.Services.AddCoreAdmin();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapDefaultControllerRoute();

app.Run();