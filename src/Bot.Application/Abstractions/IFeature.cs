using ResultSharp.Core;
using Telegram.Bot;

namespace Bot.Application.Abstractions;

public interface IFeature
{
    public string Command { get; }
    public string? LlmPrompt { get; }

    public Task<Result> ExecuteAsync(TelegramBotClient botClient, object? payload);
}
