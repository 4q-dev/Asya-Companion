using ResultSharp.Core;
using ResultSharp.Errors;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Application.Abstractions
{
    public interface IMessageHandler
    {
        public Task<Result> HandleMessageAsync(Update update, ITelegramBotClient bot, CancellationToken cancellationToken);
        public Task<Result> HandleErrorsAsync(IReadOnlyCollection<Error> errors, Update update, ITelegramBotClient bot, CancellationToken cancellationToken);
    }
}
