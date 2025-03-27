using Serilog;

namespace Bot.Api.RegistrationExtensions; 

public static class LoggingRegistration 
{
    public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddSerilog(Log.Logger);
        return services;
    }
}
