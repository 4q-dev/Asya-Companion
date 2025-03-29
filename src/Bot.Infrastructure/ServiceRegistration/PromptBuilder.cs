using Bot.Application.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Infrastructure.ServiceRegistration
{
    public static class PromptBuilder
    {
        public static void LoadPrompt(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateAsyncScope();

            var featureConatainer = scope.ServiceProvider.GetRequiredService<IFeatureContainer>();
            

        }
    }
}
