namespace Bot.Infrastructure.ServiceRegistration.ConfigurationOptions
{
    internal record LlmOptons
    {
        internal required string BaseUrl { get; init; }
    }
}
