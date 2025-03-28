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
using Telegram.Bot.Types.Enums;
using LogLevel = ResultSharp.Logging.LogLevel;

namespace Bot.Api.Controllers;

[ApiController]
[Route("/")]
public class BotController :
    ControllerBase
{

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

        // потом вынести в хелпер для логгирования, а пока похуй, вайбуем
        if (update.Type == UpdateType.Message)
            Log.Information("Получено сообщение от {user} с текстом {text} из чата {from}", update.Message!.From?.Id, update.Message.Text, update.Message.Chat);

        var result = await Result.TryAsync(async () =>
        {
            await messageHandler.HandleMessageAsync(update, bot, cancellatoinToken)
                .OnFailureAsync(errors => messageHandler.HandleErrorsAsync(errors, update, bot, cancellatoinToken))
                .LogErrorMessagesAsync();
        });

        return result.ToResponse();
    }
}