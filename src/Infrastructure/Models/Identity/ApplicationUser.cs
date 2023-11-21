using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Domain.Abstractions;
using CleanBlazor.Domain.Entities.Communication;
using Microsoft.AspNetCore.Identity;

namespace CleanBlazor.Infrastructure.Models.Identity;

public class ApplicationUser : IdentityUser<string>, IUser, IChatUser, IAuditableEntity, ISoftDeletableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePictureDataUrl { get; set; }
    public bool IsActive { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<ChatMessage<ApplicationUser>> ChatMessagesFromUsers { get; set; } =
        new HashSet<ChatMessage<ApplicationUser>>();

    public virtual ICollection<ChatMessage<ApplicationUser>> ChatMessagesToUsers { get; set; } =
        new HashSet<ChatMessage<ApplicationUser>>();
    public string CreatedBy { get; }
    public DateTimeOffset CreatedOn { get; }
    public string LastModifiedBy { get; }
    public DateTimeOffset? LastModifiedOn { get; }
    public string DeletedBy { get; private set; }
    public DateTimeOffset? DeletedOn { get; private set; }
    public bool Deleted { get; private set; }

    /// <inheritdoc />
    public void Restore()
    {
        Deleted = false;
        DeletedBy = null;
        DeletedOn = null;
    }
}
