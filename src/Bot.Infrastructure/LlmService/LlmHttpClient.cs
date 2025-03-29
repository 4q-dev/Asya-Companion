using System.Net;
using System.Net.Http.Json;
using Bot.Infrastructure.LlmService.Models;
using Bot.Infrastructure.ServiceRegistration.ConfigurationOptions;
using Microsoft.Extensions.Options;
using ResultSharp.Core;
using Serilog;
using Error = ResultSharp.Errors.Error;

namespace Bot.Infrastructure.LlmService
{
    internal class LlmHttpClient(IHttpClientFactory httpClientFactory, IOptions<LlmOptions> options)
    {
        private const string endpoint = "/chat/completions";

        private readonly HttpClient httpClient = httpClientFactory.CreateClient(nameof(LlmHttpClient));
        private readonly LlmOptions options = options.Value;

        public async Task<Result<string>> GetByPrompt(string prompt, CancellationToken cancellationToken, string? context = default)
        {
            var message = context is null
                ? prompt
                : $"{context}\n{prompt}";

            var body = new
            {
                options.Model,
                Mesages = new[]
                {
                    new
                    {
                        Role = "user",
                        Content = message
                    }
                }
            };

            Log.Debug("Отправка запроса к LLM-сервису: {body}", body);

            var response = await httpClient.PostAsJsonAsync(endpoint, body, cancellationToken);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken);
                Log.Warning("Не удалось получить ответ от LLM-сервиса. Код ответа: {code}, сообщение: {message]", response.StatusCode, error?.Error.Message);
                return Error.Failure();
            }

            var payload = await response.Content.ReadFromJsonAsync<LlmResponse>(cancellationToken);
            if (payload is null)
            {
                Log.Warning("Ответ от LLM-сервиса пуст");
                return Error.Failure();
            }

            var content = payload.Choises.Message.Content;
            Log.Debug("Ответ от LLM-сервиса: {content}", content);
            return content;
        }
    }
}
