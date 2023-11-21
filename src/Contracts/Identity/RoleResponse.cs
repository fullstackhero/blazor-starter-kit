using System.ComponentModel.DataAnnotations;

namespace CleanBlazor.Contracts.Identity;

public class RoleResponse
{
    public string Id { get; set; }

    [Required] public string Name { get; set; }

    public string Description { get; set; }
}
