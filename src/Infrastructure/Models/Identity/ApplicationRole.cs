using CleanBlazor.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace CleanBlazor.Infrastructure.Models.Identity;

public class ApplicationRole : IdentityRole, IAuditableEntity
{
    public ApplicationRole() {}

    public ApplicationRole(string roleName, string roleDescription = null) : base(roleName)
    {
        Description = roleDescription;
    }

    public string Description { get; set; }
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = new HashSet<ApplicationRoleClaim>();
    public string CreatedBy { get; }
    public DateTimeOffset CreatedOn { get; }
    public string LastModifiedBy { get; }
    public DateTimeOffset? LastModifiedOn { get; }
}
