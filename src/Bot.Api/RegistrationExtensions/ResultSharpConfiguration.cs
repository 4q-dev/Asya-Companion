using ResultSharp.Configuration;
using ResultSharp.Logging.Serilog;
using Serilog;

namespace Bot.Api.RegistrationExtensions;

public class ResultSharpConfiguration
{
    public static void ConfigureResultSharp()
    {
        new ResultConfigurationGlobal().Configure(options =>
        {
            options.LoggingConfiguration.Configure(logConfig =>
                logConfig.LoggingAdapter = new SerilogAdapter(Log.Logger)
            );
        });
    }
}
