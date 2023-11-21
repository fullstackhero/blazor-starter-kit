using Serilog;

namespace CleanBlazor.Server.Extensions;

internal static class HostBuilderExtensions
{
    internal static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

        #if DEBUG
        Serilog.Debugging.SelfLog.Enable(Console.WriteLine);
        Serilog.Debugging.SelfLog.Enable(Console.Error);
        #endif

        return builder;
    }
}
