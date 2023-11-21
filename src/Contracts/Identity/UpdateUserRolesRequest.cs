using CleanBlazor.Shared.Models.Identity;

namespace CleanBlazor.Contracts.Identity;

public class UpdateUserRolesRequest
{
    public string UserId { get; set; }
    public IList<UserRoleModel> UserRoles { get; set; }
}
