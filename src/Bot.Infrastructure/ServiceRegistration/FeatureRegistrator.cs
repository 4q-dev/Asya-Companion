using Bot.Application.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Infrastructure.ServiceRegistration
{
    public static class FeatureRegistrator
    {
        public static void AddFeatures(this IServiceCollection services)
        {
            var assembly = typeof(IFeature).Assembly ?? throw new InvalidOperationException("Не удалось получить сборку");

            foreach (var type in assembly.GetTypes())
            {
                if (type.GetInterfaces().Any(i => i == typeof(IFeature)))
                    services.AddTransient(type);
            }
        }

        public static void UseFeatures(this IApplicationBuilder app)
        {
            var assembly = typeof(IFeature).Assembly ?? throw new InvalidOperationException("Не удалось получить сборку");

            var featuresTypes = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IFeature)));

            using var scope = app.ApplicationServices.CreateScope();

            var container = scope.ServiceProvider.GetRequiredService<IFeatureContainer>() as FeatureContainer 
                ?? throw new InvalidOperationException();
            

            foreach (var featureType in featuresTypes)
            {
                var feature = scope.ServiceProvider.GetRequiredService(featureType);

                var addMethod = (container.GetType()
                    .GetMethod(nameof(FeatureContainer.AddFeature)) ?? throw new InvalidOperationException("Не удалось получить метод для регистрации фичи."))
                    .MakeGenericMethod(featureType);

                addMethod.Invoke(container, [feature, () =>
                {
                    using var scope = app.ApplicationServices.CreateScope();
                    return (IFeature)scope.ServiceProvider.GetRequiredService(featureType);
                }]);
            }
        }
    }
}
