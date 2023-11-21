using CleanBlazor.Domain.Entities.Misc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanBlazor.Infrastructure.Data.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.Property(p => p.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.HasOne(p => p.DocumentType)
            .WithMany(p => p.Documents)
            .HasForeignKey(p => p.DocumentTypeId)
            .IsRequired();
    }
}
