using BlazorHero.CleanArchitecture.Server.Permission;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using BlazorHero.CleanArchitecture.AccountService.Interfaces;
using BlazorHero.CleanArchitecture.Application.Constants.Permission;
using BlazorHero.CleanArchitecture.AuditService.Extensions;
using BlazorHero.CleanArchitecture.AuditService.Interfaces;
using BlazorHero.CleanArchitecture.ChatService.Extensions;
using BlazorHero.CleanArchitecture.ChatService.Interfaces;
using BlazorHero.CleanArchitecture.CurrentUserService.Interfaces;
using BlazorHero.CleanArchitecture.DataAccess.Interfaces.Contexts;
using BlazorHero.CleanArchitecture.DataAccess.MsSql;
using BlazorHero.CleanArchitecture.DataAccess.MsSql.Repositories;
using BlazorHero.CleanArchitecture.DataAccess.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.DataAccess.Interfaces;
using BlazorHero.CleanArchitecture.DataAccess.MsSql.Contexts;
using BlazorHero.CleanArchitecture.DateTimeService;
using BlazorHero.CleanArchitecture.DateTimeService.Interfaces;
using BlazorHero.CleanArchitecture.Domain.Entities.Identity;
using BlazorHero.CleanArchitecture.ExcelService.Interfaces;
using BlazorHero.CleanArchitecture.IdentityService.Configurations;
using BlazorHero.CleanArchitecture.RoleService.Interfaces;
using BlazorHero.CleanArchitecture.TokenService.Interfaces;
using BlazorHero.CleanArchitecture.UploadService.Interfaces;
using BlazorHero.CleanArchitecture.UserService.Interfaces;
using BlazorHero.CleanArchitecture.Utils.Wrapper;
using BlazorHero.CleanArchitecture.MailService.Interfaces;
using BlazorHero.CleanArchitecture.RoleService.Extensions;
using BlazorHero.CleanArchitecture.SMTPMailService.Configurations;
using BlazorHero.CleanArchitecture.UserService.Extensions;

namespace BlazorHero.CleanArchitecture.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static AppConfiguration GetApplicationSettings(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppConfiguration>();
        }

        public static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //TODO - Lowercase Swagger Documents
                //c.DocumentFilter<LowercaseDocumentFilter>();
                //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f
                c.IncludeXmlComments(string.Format(@"{0}\BlazorHero.CleanArchitecture.Server.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BlazorHero.CleanArchitecture",
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }

        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddDbContext<BlazorHeroContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .AddTransient<IDatabaseSeeder, DatabaseSeeder>();

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddTransient<IBlazorHeroContext, BlazorHeroContext>()
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                .AddIdentity<BlazorHeroUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<BlazorHeroContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            services.Configure<MailConfiguration>(configuration.GetSection("MailConfiguration"));
            services.AddTransient<IMailService, SMTPMailService.SMTPMailService>();

            services.AddTransient<ITokenService, IdentityService.IdentityService>();
            services.AddTransient<IRoleService, RoleService.RoleService>();
            services.AddTransient<IAccountService, AccountService.AccountService>();
            services.AddTransient<IUserService, UserService.UserService>();
            services.AddTransient<IChatService, ChatService.ChatService>();
            services.AddTransient<IUploadService, UploadService.UploadService>();
            services.AddTransient<IAuditService, AuditService.AuditService>();
            services.AddScoped<IExcelService, ExcelService.ExcelService>();

            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddApplicationServicesMappings(this IServiceCollection services)
        {
            services.AddAuditServiceMappings();
            services.AddChatServiceMappings();
            services.AddRoleServiceMappings();
            services.AddUserServiceMappings();
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, AppConfiguration config)
        {
            var key = Encoding.ASCII.GetBytes(config.Secret);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };
                    bearer.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized."));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource."));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
            services.AddAuthorization(options =>
            {
                // Here I stored necessary permissions/roles in a constant
                foreach (var prop in typeof(Permissions).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                {
                    options.AddPolicy(prop.GetValue(null).ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, prop.GetValue(null).ToString()));
                }
            });
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, Services.CurrentUserService>();
            return services;
        }
    }
}