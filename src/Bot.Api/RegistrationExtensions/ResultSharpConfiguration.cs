using ResultSharp.Configuration;
using ResultSharp.Errors;
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

            options.ExceptionHandlerConfiguration.Configure(errConfig =>
            {
                errConfig.ExceptionHandler = exception =>
                {
                    Log.Error(exception, "An exception occurred");

                    return Error.Failure("An error occurred");
                };
            });
        });
    }
}
