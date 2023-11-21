using CleanBlazor.Shared.Models.Identity;

namespace CleanBlazor.Contracts.Identity;

public class UserRolesResponse
{
    public List<UserRoleModel> UserRoles { get; set; } = new();
}
