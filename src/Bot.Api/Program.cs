using Bot.Api.RegistrationExtensions;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddControllers();

services.AddCustomLogging(configuration);
ResultSharpConfiguration.ConfigureResultSharp();

services.AddTelegramBot(configuration);

var app = builder.Build();

//app.UseHttpsRedirection(); // пока что это нахуй не надо

await app.UseTelegamBotWebhook();

app.UseAuthorization();

app.MapControllers();

app.Run();
