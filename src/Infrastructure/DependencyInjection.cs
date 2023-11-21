using System.Reflection;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Storage;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Storage.Provider;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Infrastructure.Data;
using CleanBlazor.Infrastructure.Data.Interceptors;
using CleanBlazor.Infrastructure.Models.Identity;
using CleanBlazor.Infrastructure.Repositories;
using CleanBlazor.Infrastructure.Services;
using CleanBlazor.Infrastructure.Services.Identity;
using CleanBlazor.Infrastructure.Services.Mail;
using CleanBlazor.Infrastructure.Services.Storage;
using CleanBlazor.Infrastructure.Services.Storage.Provider;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanBlazor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddRepositories()
            .AddIdentity()
            .AddServices()
            .AddInfrastructureMappings()
            .AddServerStorage();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IRoleClaimService, RoleClaimService>();
        services.AddTransient<ITokenService, IdentityService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUploadService, UploadService>();
        services.AddTransient<IAuditService, AuditService>();
        services.AddTransient<IMailService, SmtpMailService>();
        services.AddScoped<IExcelService, ExcelService>();
        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>()
            .AddScoped<ISaveChangesInterceptor, SoftDeletableEntityInterceptor>()
            .AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>()
            .AddDbContext<ApplicationDbContext>((sp, options) => options
                .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .AddScoped<ApplicationDbContextInitializer>()
            .AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddInfrastructureMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }


    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IBrandRepository, BrandRepository>()
            .AddScoped<IDocumentRepository, DocumentRepository>()
            .AddScoped<IDocumentTypeRepository, DocumentTypeRepository>()
            .AddScoped<IChatMessageRepository, ChatMessageRepository>();

    private static IServiceCollection AddServerStorage(this IServiceCollection services)
    {
        services
            .AddScoped<IStorageProvider, ServerStorageProvider>()
            .AddScoped<IServerStorageService, ServerStorageService>()
            .AddScoped<ISyncServerStorageService, ServerStorageService>();

        return services;
    }
}
