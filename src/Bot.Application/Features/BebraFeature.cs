using Bot.Application.Abstractions;
using Bot.Application.Models;
using Microsoft.Extensions.Logging;
using ResultSharp.Core;
using Telegram.Bot;

namespace Bot.Application.Features
{
    public class BebraFeature(ILogger<BebraFeature> testDepends) :
        IFeature
    {
        public string Command => "/bebra";

        public string? LlmPrompt => "Данная фича является рофлом. пользователь может запросить эту фичу используя ключевые слова и фразы: бебра, беброчка, беберовка, отправька беброчки и тд";

        public async Task<Result> ExecuteAsync(ITelegramBotClient botClient, UserContext context, CancellationToken cancellationToken)
        {
            await botClient.SendMessage(context.RecievedMessage.Chat.Id, "Бебра отправлена", cancellationToken: cancellationToken);
            testDepends.LogInformation("ХУЙ!");
            return Result.Success();
        }
    }
}
