using Bot.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Infrastructure.ServiceRegistration
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IContextManager, InMemoryContextManager>();
            services.AddSingleton<IFeatureContainer, FeatureContainer>();
            services.AddFeatures();

            return services;
        }
    }
}
