using System.Security.Claims;
using System.Text;
using CleanBlazor.Application.Configuration;
using CleanBlazor.Shared.Constants.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace CleanBlazor.Server.Configuration;

internal class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IOptions<AppConfiguration> _appConfiguration;

    public JwtBearerOptionsSetup(IOptions<AppConfiguration> appConfiguration)
    {
        _appConfiguration = appConfiguration;
    }

    public void Configure(JwtBearerOptions options)
    {
        var key = Encoding.UTF8.GetBytes(_appConfiguration.Value.Secret);

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                StringValues accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                PathString path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments(ApplicationConstants.SignalR.HubUrl))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
