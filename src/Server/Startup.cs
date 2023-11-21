using CleanBlazor.Application;
using CleanBlazor.Contracts;
using CleanBlazor.Infrastructure;
using CleanBlazor.Infrastructure.Data;
using CleanBlazor.Server.Extensions;
using CleanBlazor.Server.Filters;
using CleanBlazor.Server.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace CleanBlazor.Server;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLocalization(options => { options.ResourcesPath = Path.Combine("Configuration", "Resources"); });

        services.AddApplication(_configuration);
        services.AddInfrastructure(_configuration);
        services.AddContracts();
        services.AddServer(_configuration);

        services.AddScoped<ServerPreferenceManager>();
        services.AddSignalR();
        services.AddHangfire(x => x.UseSqlServerStorage(_configuration.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();
        services.AddControllers();
        services.AddFluentValidationAutoValidation();
        services.AddRazorPages();
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
        services.AddLazyCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        app.UseForwarding(_configuration);
        app.UseExceptionHandling(env);
        app.UseHttpsRedirection();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
            RequestPath = new PathString("/Files")
        });
        app.UseServerCultureFromPreferences(serviceProvider);
        app.UseRequestLocalizationByCulture();
        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHangfireDashboard("/jobs",
            new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
        app.UseEndpoints();
        app.ConfigureSwagger();
        app.InitializeDatabaseAsync().GetAwaiter().GetResult();
    }
}
