using Bot.Application.Abstractions;
using Bot.Application.Models;
using ResultSharp.Core;
using ResultSharp.Errors;
using ResultSharp.Extensions.FunctionalExtensions.Async;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Application
{
    internal class MessageHandler(IContextManager contextManager, IFeatureContainer featureContainer) : IMessageHandler
    {
        private readonly IContextManager contextManager = contextManager;
        private readonly IFeatureContainer featureContainer = featureContainer;

        public async Task<Result> HandleErrorsAsync(IReadOnlyCollection<Error> errors, Update update, ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            // потом можно так же чере фичи хендлить, пока что похуй, говнокодим
            //await bot.SendMessage(update.Message!.Chat.Id, $"Произошла ошибка: {errors.First().Message}", cancellationToken: cancellationToken);
            return Result.Success();
        }

        public async Task<Result> HandleMessageAsync(Update update, ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            //UserContext context = null;
            var context = await contextManager.GetUserContextAsync(update, cancellationToken);

            var hui = update.Type switch
            {
                UpdateType.Message => await HandleMessageAsync(update.Message, context, bot, cancellationToken),
                UpdateType.CallbackQuery => await HandleCallbackQuery(update.CallbackQuery, context, bot, cancellationToken),
                _ => Result.Failure(Error.BadRequest("Неподдерживаемый тип сообщения."))
            };

            return hui;
        }

        private async Task<Result> HandleMessageAsync(Message? message, UserContext context, ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            if (message is null || string.IsNullOrWhiteSpace(message.Text))
                return Error.BadRequest("Сообщение не может быть пустым.");

            if (message.Text[0] != '/') // все команды обязаны начинаться с '/'
                return Error.BadRequest("Некорректный формат команды."); // потом будет запрос к ллм

            var command = message.Text.Split(' ')[0];
            var result = await featureContainer
                .GetFeature(command)
                .ThenAsync(feature => feature.ExecuteAsync(bot, context, cancellationToken));

            return result;
        }

        private async Task<Result> HandleCallbackQuery(CallbackQuery? callbackQuery, UserContext context, ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
