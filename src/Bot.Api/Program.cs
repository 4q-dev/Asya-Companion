using Bot.Api.RegistrationExtensions;
using Bot.Infrastructure.ServiceRegistration;
using Bot.Application.ServiceRegistration;
using System.Diagnostics;
using Serilog;

var sw = new Stopwatch();
sw.Start();

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddControllers();

services.AddCustomLogging(configuration);
ResultSharpConfiguration.ConfigureResultSharp();

services
    .AddInfrastructure()
    .AddApplication();

services.AddTelegramBot(configuration);

var app = builder.Build();

//app.UseHttpsRedirection(); // пока что это нахуй не надо

var webhookTask = app.UseTelegamBotWebhook();

app.UseFeatures();

app.UseAuthorization();

app.MapControllers();

await webhookTask;

sw.Stop();
Log.Information("Подготовка к запуску заняла {elapsedTime}ms", sw.ElapsedMilliseconds);

app.Run();
