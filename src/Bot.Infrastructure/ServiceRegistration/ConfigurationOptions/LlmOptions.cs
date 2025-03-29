namespace Bot.Infrastructure.ServiceRegistration.ConfigurationOptions
{
    internal record LlmOptions
    {
        private const string ApiKeyEnvName = "ZAZAGRAM_GROQ_TOKEN";
        internal static readonly string ApiKey = Environment.GetEnvironmentVariable(ApiKeyEnvName, EnvironmentVariableTarget.Machine)
            ?? throw new InvalidOperationException($"Необходимо поместить токен от api groq в переменную окружения '{ApiKeyEnvName}'");
        internal required string BaseUrl { get; init; }
        internal required string Model { get; init; }
    }
}
