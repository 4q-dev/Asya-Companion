using Bot.Application.Abstractions;
using Bot.Application.Models;
using ResultSharp.Core;
using Telegram.Bot;

namespace Bot.Application.Features
{
    internal class StartFeature : IFeature
    {
        public string Command => "/start";

        public string? LlmPrompt => "throw new NotImplementedException()";

        public async Task<Result> ExecuteAsync(ITelegramBotClient botClient, UserContext context, CancellationToken cancellationToken)
        {
            await botClient.SendMessage(context.RecievedMessage.Chat.Id, "Кто я... Аянами Рей", cancellationToken: cancellationToken);

            return Result.Success();
        }
    }
}
