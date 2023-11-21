using CleanBlazor.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanBlazor.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(p => p.Rate)
            .HasPrecision(18, 2);

        builder.HasOne(p => p.Brand)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.BrandId)
            .IsRequired();
    }
}
