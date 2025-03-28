using Bot.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Application.ServiceRegistration
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IMessageHandler, MessageHandler>();

            return services;
        }
    }
}
