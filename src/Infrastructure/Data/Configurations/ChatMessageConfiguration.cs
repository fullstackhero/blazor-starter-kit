using CleanBlazor.Domain.Entities.Communication;
using CleanBlazor.Infrastructure.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanBlazor.Infrastructure.Data.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage<ApplicationUser>>
{
    public void Configure(EntityTypeBuilder<ChatMessage<ApplicationUser>> builder)
    {
        builder.ToTable("ChatMessages");

        builder.Property(p => p.Message)
            .HasMaxLength(8000);

        builder.HasOne(d => d.FromUser)
            .WithMany(p => p.ChatMessagesFromUsers)
            .HasForeignKey(d => d.FromUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(d => d.ToUser)
            .WithMany(p => p.ChatMessagesToUsers)
            .HasForeignKey(d => d.ToUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasQueryFilter(p => !p.FromUser.Deleted && !p.ToUser.Deleted);
    }
}
