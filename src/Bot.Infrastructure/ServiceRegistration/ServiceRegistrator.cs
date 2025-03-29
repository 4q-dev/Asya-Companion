using Bot.Application.Abstractions;
using Bot.Infrastructure.LlmService;
using Bot.Infrastructure.ServiceRegistration.ConfigurationOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Infrastructure.ServiceRegistration
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IContextManager, InMemoryContextManager>();
            services.AddSingleton<IFeatureContainer, FeatureContainer>();
            services.AddFeatures();

            services.AddLlmServives(configuration);

            return services;
        }

        private static IServiceCollection AddLlmServives(this IServiceCollection services, IConfiguration configuration)
        {
            var llmOptionsSection = configuration.GetSection(nameof(LlmOptions));
            var llmOptions = llmOptionsSection.Get<LlmOptions>() 
                ?? throw new InvalidOperationException($"Не удалось получить конфигруацию для {nameof(LlmOptions)}");
            services.Configure<LlmOptions>(llmOptionsSection);

            var httpClient = services.AddHttpClient(nameof(LlmHttpClient), (configure) =>
            {
                configure.BaseAddress = new Uri(llmOptions.BaseUrl);
                configure.DefaultRequestHeaders.Add("Authorization", $"Bearer {LlmOptions.ApiKey}");
            });

            services.AddSingleton<LlmContext>();

            return services;
        }
    }
}
