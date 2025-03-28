using ResultSharp.Core;

namespace Bot.Application.Abstractions;

public interface ILlmService
{
    public Task<Result<string>> GetCommandFromContextAsync(string userMessage, CancellationToken cancellationToken);
}
