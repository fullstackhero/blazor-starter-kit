using System.Reflection;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Domain.Entities.Catalog;
using CleanBlazor.Domain.Entities.Communication;
using CleanBlazor.Domain.Entities.Misc;
using CleanBlazor.Infrastructure.Data.Extensions;
using CleanBlazor.Infrastructure.Models.Audit;
using CleanBlazor.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanBlazor.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim,
    IdentityUserToken<string>>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Audit> AuditTrails { get; set; }
    public DbSet<ChatMessage<ApplicationUser>> ChatMessages { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await Database.BeginTransactionAsync(cancellationToken);
        return new EfTransaction(transaction);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyAuditableEntityConfiguration();
        builder.ApplySoftDeletableEntityConfiguration();
    }
}
