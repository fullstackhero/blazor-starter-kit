using System.Net;
using System.Reflection;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Configuration;
using CleanBlazor.Server.Configuration;
using CleanBlazor.Server.Localization;
using CleanBlazor.Server.Services;
using CleanBlazor.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CleanBlazor.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddServer(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddForwarding(configuration)
            .AddServerLocalization()
            .RegisterSwagger()
            .AddCurrentUserService()
            .AddJwtAuthentication();

        return services;
    }

    private static IServiceCollection AddForwarding(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>();
        if (!config.BehindSslProxy)
        {
            return services;
        }

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            if (string.IsNullOrWhiteSpace(config.ProxyIp))
            {
                return;
            }

            var ipCheck = config.ProxyIp;
            if (IPAddress.TryParse(ipCheck, out IPAddress proxyIp))
            {
                options.KnownProxies.Add(proxyIp);
            }
            else
            {
                Log.Warning("Invalid Proxy IP of {IpCheck}, Not Loaded", ipCheck);
            }
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(config.ApplicationUrl.TrimEnd('/'));
                });
        });

        return services;
    }

    private static IServiceCollection AddServerLocalization(this IServiceCollection services)
    {
        services.TryAddTransient(typeof(IStringLocalizer<>), typeof(ServerLocalizer<>));
        return services;
    }

    private static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // include all project's xml comments
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    var xmlFile = $"{assembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(baseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
            }

            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CleanBlazor",
                    License = new OpenApiLicense
                    {
                        Name = "MIT License", Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description =
                        "Input your Bearer token in this format - Bearer {your token here} to access this API"
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.AddAuthorization(options =>
        {
            Permissions.GetRegisteredPermissions().ForEach(permission => options.AddPolicy(permission,
                policy => policy.RequireClaim(ApplicationClaimTypes.Permission, permission)));
        });
        return services;
    }
}
