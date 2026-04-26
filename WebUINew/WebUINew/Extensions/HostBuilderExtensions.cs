using Serilog;

namespace WebUINew.Extensions;

public static class HostBuilderExtensions
{
    //TODO: Why not used anymore?
    public static IHostBuilder UseSerilog(this IHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        SerilogHostBuilderExtensions.UseSerilog(builder);
        return builder;
    }
}