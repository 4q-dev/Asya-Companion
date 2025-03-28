using Bot.Application.Abstractions;
using ResultSharp.Core;

namespace Bot.Infrastructure;

public class LlmService(IHttpClientFactory httpClientFactory) :
    ILlmService
{
    private readonly HttpClient httpClient = httpClientFactory

    public Task<Result<string>> GetCommandFromContextAsync(string userMessage, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
