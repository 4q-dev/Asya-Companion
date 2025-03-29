namespace Bot.Api.ConfigurationOptions;

public class BotOptions
{
    private const string SecretTokenEnvName = "ZAZAGRAM_SECRET_TOKEN";
    private const string BotTokenEnvName = "ZAZAGRAM_BOT_TOKEN";

    public static readonly string SecretToken = Environment.GetEnvironmentVariable(SecretTokenEnvName, EnvironmentVariableTarget.Machine)
        ?? throw new InvalidOperationException($"Необходимо поместить секретный токен в переменную окружения '{SecretTokenEnvName}'");

    public static readonly string BotToken = Environment.GetEnvironmentVariable(BotTokenEnvName, EnvironmentVariableTarget.Machine)
        ?? throw new InvalidOperationException($"Необходимо поместить токен бота в переменную окружения '{BotTokenEnvName}'");

    public required string WebhookUrl { get; set; }
}
