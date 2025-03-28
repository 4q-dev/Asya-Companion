using Bot.Application.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
            var sw = new Stopwatch();
            sw.Start();

            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(FeatureRegistrator));
            var assembly = typeof(IFeature).Assembly ?? throw new InvalidOperationException("Не удалось получить сборку");

            logger.LogInformation("Осуществляю поиск фич в сборке '{assembly}'...", assembly);

            var featuresTypes = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IFeature)));

            logger.LogInformation("Найдено фич: {cnt}", featuresTypes.Count());

            using var scope = app.ApplicationServices.CreateScope();
            

            var container = scope.ServiceProvider.GetRequiredService<IFeatureContainer>() as FeatureContainer 
                ?? throw new InvalidOperationException();
            

            foreach (var featureType in featuresTypes)
            {
                var feature = scope.ServiceProvider.GetRequiredService(featureType) as IFeature 
                    ?? throw new InvalidCastException($"{featureType.Name} должен реализовывать {nameof(IFeature)}");

                logger.LogInformation("Регистрация фичи {name} для командой {command}", featureType.Name, feature.Command);
                container.AddFeature(feature, () =>
                {
                    using var scope = app.ApplicationServices.CreateAsyncScope();
                    return (IFeature)scope.ServiceProvider.GetRequiredService(featureType);
                });
            }

            sw.Stop();
            logger.LogInformation("Фичи зарегистрированны, затрачено {elapsedTime}ms", sw.ElapsedMilliseconds);
        }
    }
}
