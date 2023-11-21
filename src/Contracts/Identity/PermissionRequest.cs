namespace CleanBlazor.Contracts.Identity;

public class PermissionRequest
{
    public string RoleId { get; set; }
    public IList<RoleClaimRequest> RoleClaims { get; set; }
}
