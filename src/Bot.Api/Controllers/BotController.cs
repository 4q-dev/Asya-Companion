using Bot.Api.ConfigurationOptions;
using Bot.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ResultSharp.Core;
using ResultSharp.Extensions.FunctionalExtensions.Async;
using ResultSharp.HttpResult;
using ResultSharp.Logging;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Api.Controllers;

[ApiController]
[Route("/")]
public class BotController :
    ControllerBase
{ // блять какая же хуйня так скобки ставить это пиздец дима, что это нахуй за слипшийся кусок говна
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
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, IMessageHandler messageHandler, CancellationToken cancellatoinToken)
    {
        if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != secretToken)
            return Forbid();

        Log.Information("Получено сообщение от {user} с текстом {text} из чата {from}", update.Message.From.Id, update.Message.Text, update.Message.Chat);

        var result = await Result.TryAsync(async () =>
        { 
            await messageHandler.HandleMessageAsync(update, bot, cancellatoinToken)
                .OnFailureAsync(errors => messageHandler.HandleErrorsAsync(errors, update, bot, cancellatoinToken))
                .LogErrorMessagesAsync();
        });

        return result.ToResponse();
    }
}