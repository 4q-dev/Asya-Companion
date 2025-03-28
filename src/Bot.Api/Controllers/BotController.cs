using Bot.Api.ConfigurationOptions;
using Microsoft.AspNetCore.Mvc;
using ResultSharp.Core;
using ResultSharp.Extensions.FunctionalExtensions.Async;
using Telegram.Bot;
using Telegram.Bot.Types;
using Bot.Application.Abstractions;
using ResultSharp.Logging;

namespace Bot.Api.Controllers;

[ApiController]
[Route("/")]
public class BotController : 
    ControllerBase { // блять какая же хуйня так скобки ставить это пиздец дима, что это нахуй за слипшийся кусок говна
    private static readonly string secretToken = Environment.GetEnvironmentVariable(BotOptions.SecretTokenEnvName, EnvironmentVariableTarget.Machine)
        ?? throw new InvalidOperationException($"Необходимо поместить токен в переменную окружения '{BotOptions.SecretTokenEnvName}'");

    /// <summary>
    /// Все обновления от тг будут приходить сюда.
    /// </summary>
    /// <param name="update">Объект <see cref="Update"/> приходящий от телеграмма</param>
    /// <param name="bot">Зарегистрированный объект клиента от либы Telegram.Bot</param>
    /// <param name="cancellatoinToken">Ну это токен и так понятно</param>
    /// <returns>Ничего</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, IMessageHandler updateHandler, CancellationToken cancellatoinToken) {
        if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != secretToken)
            return Forbid();

        await Result.TryAsync(async () =>
        {
            await updateHandler.HandleMessageAsync(update, bot, cancellatoinToken);
        })
        .OnFailureAsync(errors => updateHandler.HandleErrorsAsync(errors, bot, cancellatoinToken))
        .LogErrorMessagesAsync();

        return Ok();
    }
}
