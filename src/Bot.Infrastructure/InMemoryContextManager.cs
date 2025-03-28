using Bot.Application.Abstractions;
using Bot.Application.Models;
using Bot.Common.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Infrastructure;

internal class InMemoryContextManager :
    IContextManager
{
    private readonly Dictionary<long, UserContext> userContexts = [];

    // в будущем это можно перенести в условный редис
    public async Task<UserContext> GetUserContextAsync(Update update, CancellationToken cancellationToken)
    {
        var userId = update.GetUserId();
        if (userContexts.TryGetValue(userId, out var userContext))
            return UpdateContext(update, userContext);

        var context = new UserContext();
        UpdateContext(update, context);
        userContexts.Add(userId, context);

        return context;
    }

    public UserContext UpdateContext(Update update, UserContext context)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                context.MessageHistory.Add(update.Message!);
                break;

            case UpdateType.CallbackQuery:
                context.CallbackQueryHistory.Add(update.CallbackQuery!);
                break;

            default:
                throw new InvalidOperationException("Неподдерживаемый тип сообщения");
        }

        return context;
    }
}
