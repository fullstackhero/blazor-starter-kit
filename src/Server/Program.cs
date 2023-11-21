using CleanBlazor.Infrastructure.Data;
using CleanBlazor.Server.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Context;

namespace CleanBlazor.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        // The initial "bootstrap" logger is able to log errors during start-up. It's completely replaced by the
        // logger configured in `ConfigureSerilog()` below, once configuration and dependency-injection have both been
        // set up successfully.
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        using var _ = LogContext.PushProperty("SourceContext", typeof(Program).FullName);

        Log.Information("Starting up...");

        try
        {
            IHost host = CreateHostBuilder(args).Build();

            await RunMigrationsAsync(host);

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.Information("Shutting down...");
            await Log.CloseAndFlushAsync();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStaticWebAssets();
                webBuilder.UseStartup<Startup>();
            });

    private static async Task RunMigrationsAsync(IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            if (context.Database.IsSqlServer())
            {
                Log.Information("Running database migration(s).");
                await context.Database.MigrateAsync();
                Log.Information("Completed database migration(s).");
            }
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred while migrating or seeding the database.");
            throw;
        }
    }
}
