using Bot.Api.ConfigurationOptions;

using Microsoft.Extensions.Options;

using Telegram.Bot;

namespace Bot.Api.RegistrationExtensions;

public static class TelegramBotRegistrator
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(BotOptions));
        var botConfiguration = section.Get<BotOptions>();
        services.Configure<BotOptions>(section);
        var token = Environment.GetEnvironmentVariable(BotOptions.BotTokenEnvName, EnvironmentVariableTarget.Machine) ?? throw new InvalidOperationException($"Необходимо поместить токен в переменную окружения '{BotOptions.BotTokenEnvName}'");

        services.AddHttpClient(nameof(BotOptions)).AddTypedClient<ITelegramBotClient>(
            httpClient => new TelegramBotClient(token, httpClient));

        services.ConfigureTelegramBotMvc();

        return services;
    }

    public static async Task UseTelegamBotWebhook(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IOptions<BotOptions>>().Value;
        var secretToken = Environment.GetEnvironmentVariable(BotOptions.SecretTokenEnvName, EnvironmentVariableTarget.Machine) ?? throw new InvalidOperationException($"Необходимо поместить токен в переменную окружения '{BotOptions.SecretTokenEnvName}'");

        var botClient = app.ApplicationServices.GetRequiredService<ITelegramBotClient>();
        await botClient.SetWebhook(configuration.WebhookUrl, secretToken: secretToken, dropPendingUpdates: true);
    }
}
