using Bot.Application.Abstractions;
using ResultSharp.Core;

namespace Bot.Infrastructure.LlmService;

internal class LlmService(LlmHttpClient llmClient, LlmContext context) :
    ILlmService
{
    private readonly LlmHttpClient llmClient = llmClient;
    private readonly LlmContext context = context;

    public Task<Result<string>> GetCommandFromContextAsync(string userMessage, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
