using Microsoft.AspNetCore.Mvc;

using Serilog;

using Telegram.Bot;
using Telegram.Bot.Types;
using Bot.Api.ConfigurationOptions;

namespace Bot.Api.Controllers;

[ApiController]
[Route("/")]
public class BotController : ControllerBase
{
    private static readonly string secretToken = Environment.GetEnvironmentVariable(BotOptions.SecretTokenEnvName, EnvironmentVariableTarget.Machine) 
        ?? throw new InvalidOperationException($"Необходимо поместить токен в переменную окружения '{BotOptions.SecretTokenEnvName}'");

    /// <summary>
    /// Все обновления от тг будут приходить сюда.
    /// </summary>
    /// <param name="update">Объект <see cref="Update"/> приходящий от телеграмма</param>
    /// <param name="bot">Зарегистрированный объект клиента от либы Telegram.Bot</param>
    /// <param name="ct">Ну это токен и так понятно</param>
    /// <returns>Ничего</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, CancellationToken ct)
    {
        if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != secretToken)
            return Forbid();

        try
        {
            Log.Information("Received update: {Update}", update);
        }
        catch (Exception)
        {
            // а мни пихуй
        }

        return Ok();
    }
}
