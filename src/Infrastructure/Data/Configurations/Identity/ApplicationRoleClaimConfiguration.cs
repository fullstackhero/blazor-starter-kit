using CleanBlazor.Infrastructure.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanBlazor.Infrastructure.Data.Configurations.Identity;

public class ApplicationRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder.ToTable("RoleClaims", "Identity");

        builder.HasOne(d => d.Role)
            .WithMany(p => p.RoleClaims)
            .HasForeignKey(d => d.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
