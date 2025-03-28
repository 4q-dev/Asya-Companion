namespace Bot.Api.ConfigurationOptions;

public class BotOptions
{
    public const string SecretTokenEnvName = "ZAZAGRAM_SECRET_TOKEN";
    public const string BotTokenEnvName = "ZAZAGRAM_BOT_TOKEN";

    public required string WebhookUrl { get; set; }
}
